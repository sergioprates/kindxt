using Kindxt.Extensions;

namespace Kindxt.Charts
{
    public abstract class HelmChartBase
    {
        public virtual void Install(string chartName,
            string repoName, 
            string repoUrl, 
            string releaseName,
            string? configPath = null,
            string @namespace = "default")
        {
            string configCommand = string.IsNullOrWhiteSpace(configPath) ? "" : $" -f {configPath}";
            new ProcessWrapper("helm")
                .ExecuteCommand($"repo add {repoName} {repoUrl}", ignoreError: true)
                .ExecuteCommand("repo update", ignoreError: true)
                .ExecuteCommand($"uninstall {releaseName} -n {@namespace}", ignoreError: true)
                .ExecuteCommand($"install {releaseName} {chartName} -n {@namespace} --wait --debug --timeout=5m {configCommand}", 
                    KindxtPath.GetProcessPath());
        }
    }
}
