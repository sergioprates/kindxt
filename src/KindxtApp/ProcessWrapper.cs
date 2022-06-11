using System.Diagnostics;
using System.Runtime.InteropServices;

namespace KindxtApp;
public class ProcessWrapper
{
    private readonly string _file;
    private Process? _process;

    public ProcessWrapper(string file)
    {
        _file = file;
    }
    public ProcessWrapper ExecuteCommand(string arguments, string workingDirectory = "", bool ignoreError = false)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = FindExecutable(_file),
            Arguments = arguments,
            UseShellExecute = false,
            WorkingDirectory = workingDirectory,
            WindowStyle = ProcessWindowStyle.Hidden,
            RedirectStandardOutput = true
        };

        var process = Process.Start(processStartInfo);

        if (process == null)
            throw new Exception($"The process failed to start: {_file} {arguments}");

        process.WaitForExit();
        _process = process;

        if (process.ExitCode != 0 && ignoreError == false)
            throw new Exception($"The process failed to start: {_file} {arguments}. Verify if your docker is running.");

        return this;

    }

    public string? ReadOutput()
    {
        return _process?.StandardOutput.ReadToEnd();
    }

    public ProcessWrapper StartProccess(string arguments, string workingDirectory,
        bool useShell = true)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = FindExecutable(_file),
            Arguments = arguments,
            UseShellExecute = useShell,
            WorkingDirectory = workingDirectory,
            WindowStyle = ProcessWindowStyle.Hidden
        };

        _process = Process.Start(processStartInfo);
        return this;
    }

    private string FindExecutable(string name) =>
        Environment.GetEnvironmentVariable("PATH")?.Split(Path.PathSeparator)
            .Select(p => Path.Combine(p, name))
            .Select(p =>
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ? new[] { $"{p}.cmd", $"{p}.exe" }
                    : new[] { p })
            .SelectMany(a => a)
            .FirstOrDefault(f => File.Exists(f)) ??
        throw new FileNotFoundException("Could not find executable.", name);

    public void StopProcess()
    {
        if (_process == null || _process.Id == 0)
            return;

        if (!_process.HasExited)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                KillWindowsProcess(_process.Id);
            }
            else
            {
                KillUnixProcess(_process.Id);
            }
        }

        _process.Dispose();
    }

    private static void KillWindowsProcess(int processId)
    {
        using var killer =
            Process.Start(
                new ProcessStartInfo("taskkill.exe", $"/PID {processId} /T /F")
                {
                    UseShellExecute = false
                });
        killer?.WaitForExit(2000);
    }

    private static void KillUnixProcess(int processId)
    {
        using (var idGetter =
            Process.Start(new ProcessStartInfo("ps", $"-o pid= --ppid {processId}")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true
            }))
        {
            var exited = idGetter != null && idGetter.WaitForExit(2000);
            if (exited && idGetter!.ExitCode == 0)
            {
                var stdout = idGetter.StandardOutput.ReadToEnd();

                var pids = stdout.Split("\n")
                    .Where(pid => !string.IsNullOrEmpty(pid))
                    .Select(int.Parse)
                    .ToList();
                foreach (var pid in pids)
                    KillUnixProcess(pid);
            }
        }

        Process.GetProcessById(processId).Kill();
    }
}
