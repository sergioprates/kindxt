using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Kindxt.Processes;
public class ProcessWrapper
{
    private readonly string _file;
    private Process? _process;

    public ProcessWrapper(string file)
    {
        _file = file;
    }
    public virtual ProcessWrapper ExecuteCommand(string arguments, string workingDirectory = "", bool ignoreError = false, int timeout = 300000)
    {
        var process = new Process();
        process.StartInfo.FileName = FindExecutable(_file);
        process.StartInfo.Arguments = arguments;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.WorkingDirectory = workingDirectory;
        process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

        StringBuilder output = new StringBuilder();
        StringBuilder error = new StringBuilder();

        using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
        using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
        {
            process.OutputDataReceived += (sender, e) =>
            {
                if (e.Data == null)
                {
                    outputWaitHandle.Set();
                }
                else
                {
                    Console.WriteLine(e.Data);
                    output.AppendLine(e.Data);
                }
            };
            process.ErrorDataReceived += (sender, e) =>
            {
                if (e.Data == null)
                {
                    errorWaitHandle.Set();
                }
                else
                {
                    Console.WriteLine(e.Data);
                    error.AppendLine(e.Data);
                }
            };

            if (!process.Start())
                throw new Exception($"The process failed to start: {_file} {arguments}");

            _process = process;
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            if (process.WaitForExit(timeout) &&
                outputWaitHandle.WaitOne(timeout) &&
                errorWaitHandle.WaitOne(timeout))
            {
                if (process.ExitCode != 0 && ignoreError == false)
                    throw new Exception($"The process failed to start: {_file} {arguments}. Verify if your docker is running.");
            }
            else
            {
                throw new Exception($"The process exceded the maximum timeout: {_file} {arguments}.");
            }
        }

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
