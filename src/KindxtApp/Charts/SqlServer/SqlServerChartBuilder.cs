namespace KindxtApp.Charts.SqlServer
{
    public class SqlServerChartBuilder : IHelmChart
    {
        private readonly string _releaseName = "sqlserver";
        public ProcessWrapper Install()
        {
            var configDirectory = Path.Combine("Charts", "SqlServer");
            new ProcessWrapper("kind", $"load docker-image mcr.microsoft.com/mssql/server:2019-GA-ubuntu-16.04")
                .ExecuteCommand();
            new ProcessWrapper("helm", $"uninstall {_releaseName}")
                .ExecuteCommand(ignoreError: true);
            var process = new ProcessWrapper("helm", $"install {_releaseName} stable/mssql-linux -f config.yaml")
                .ExecuteCommand(configDirectory);

            new ProcessWrapper("kubectl", $"apply -f service.yaml")
                .ExecuteCommand(configDirectory);

            return process;
        }
    }
}
