using Kindxt.Extensions;
using Kindxt.Processes;

namespace Kindxt.Charts
{
    public abstract class HelmChartBase
    {
        private readonly HelmProcess _helmProcess;

        public HelmChartBase(HelmProcess helmProcess)
        {
            _helmProcess = helmProcess;
        }
        public virtual void InstallFromRepo(string chartName,
            string repoName,
            string repoUrl,
            string releaseName,
            string? configPath = null,
            string @namespace = "default",
            string parameters = "")
        {
            var configCommand = string.IsNullOrWhiteSpace(configPath) ? "" : $"-f {configPath}";
            var versionCommand = string.IsNullOrWhiteSpace(parameters) ? "" : parameters;

            var versionCommand = string.IsNullOrWhiteSpace(parameters) ? "" : parameters;

            _helmProcess
                .ExecuteCommand($"repo add {repoName} {repoUrl}", ignoreError: true)
                .ExecuteCommand("repo update", ignoreError: true)
                .ExecuteCommand($"uninstall {releaseName} -n {@namespace}", ignoreError: true)
                .ExecuteCommand($"install {releaseName} {chartName} -n {@namespace} --wait --debug --timeout=5m {configCommand} {versionCommand}".Trim(),
                    KindxtPath.GetProcessPath());
        }
    }
}
