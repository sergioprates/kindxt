namespace KindxtApp.Charts.SqlServer
{
    public class SqlServerChartBuilder : IHelmChart
    {
        private readonly string _releaseName = "sqlserver";
        public void Install()
        {
            var configDirectory = Path.Combine("Charts", "SqlServer");

            new ProcessWrapper("kind")
                .ExecuteCommand($"load docker-image mcr.microsoft.com/mssql/server:2019-GA-ubuntu-16.04");

            new ProcessWrapper("helm")
                .ExecuteCommand("repo add stable https://charts.helm.sh/stable", ignoreError: true)
                .ExecuteCommand("repo update", ignoreError: true)
                .ExecuteCommand($"uninstall {_releaseName}", ignoreError: true)
                .ExecuteCommand($"install {_releaseName} stable/mssql-linux -f config.yaml", configDirectory);
        }
    }
}
