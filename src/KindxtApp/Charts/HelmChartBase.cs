namespace Kindxt.Charts
{
    public abstract class HelmChartBase
    {
        public virtual void Install(string chartName,
            string repoName, 
            string repoUrl, 
            string releaseName,
            string configDirectory) 
        {
            new ProcessWrapper("helm")
                .ExecuteCommand($"repo add {repoName} {repoUrl}", ignoreError: true)
                .ExecuteCommand("repo update", ignoreError: true)
                .ExecuteCommand($"uninstall {releaseName}", ignoreError: true)
                .ExecuteCommand($"install {releaseName} {chartName} --wait --timeout=3m -f config.yaml", 
                    Path.Combine(Path.GetDirectoryName(Environment.ProcessPath!)!, configDirectory));
        }
    }
}
