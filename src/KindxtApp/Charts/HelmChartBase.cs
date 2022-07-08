using Kindxt.Extensions;

namespace Kindxt.Charts
{
    public abstract class HelmChartBase
    {
        public virtual void InstallFromRepo(string chartName,
            string repoName, 
            string repoUrl, 
            string releaseName,
            string chartDirectory) 
        {
            new ProcessWrapper("helm")
                .ExecuteCommand($"repo add {repoName} {repoUrl}", ignoreError: true)
                .ExecuteCommand("repo update", ignoreError: true)
                .ExecuteCommand($"uninstall {releaseName}", ignoreError: true)
                .ExecuteCommand($"install {releaseName} {chartName} --wait --debug --timeout=5m -f config.yaml", 
                    Path.Combine(KindxtPath.GetProcessPath(), chartDirectory), ignoreError: true);
        }
        public virtual void InstallFromLocalChart(string chartPath,
            string releaseName) 
        {
            new ProcessWrapper("helm")
                .ExecuteCommand($"uninstall {releaseName}", ignoreError: true)
                .ExecuteCommand($"install {releaseName} {Path.Combine(KindxtPath.GetProcessPath(), chartPath)} --wait --debug --timeout=5m", 
                    Path.Combine(KindxtPath.GetProcessPath(), chartPath), ignoreError:true);
        }
    }
}
