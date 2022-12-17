using Kindxt.Kind;
using Kindxt.Processes;

namespace Kindxt.Charts.SqlServer;
public class SqlServerChart : HelmChartBase, IHelmChart
{
    public SqlServerChart(HelmProcess helmProcess) : base(helmProcess)
    { }
    public void Install()
    {
        var configDirectory = Path.Combine("Charts", "SqlServer", "config.yaml");

        base.InstallFromRepo("stable/mssql-linux",
            "stable",
            "https://charts.helm.sh/stable",
            "sqlserver",
            configDirectory);
    }

    public string[] Parameters => new[] { "--sqlserver", "-sql" };
    public string Description => "Install sqlserver chart on kind";
    public ExtraPortMapping[] GetPortMapping() => Ports.SqlServer;
}
