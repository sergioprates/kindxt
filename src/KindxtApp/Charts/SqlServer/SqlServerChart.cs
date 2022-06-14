namespace Kindxt.Charts.SqlServer;
public class SqlServerChart : HelmChartBase, IHelmChart
{
    public void Install()
    {
        var configDirectory = Path.Combine("Charts", "SqlServer");

        base.Install("stable/mssql-linux",
            "stable",
            "https://charts.helm.sh/stable",
            "sqlserver",
            configDirectory);
    }
}
