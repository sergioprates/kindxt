using Kindxt.Kind;
using Kindxt.Processes;

namespace Kindxt.Charts.Adminer;

public class PgAdminChart : HelmChartBase, IHelmChart
{
    public PgAdminChart(HelmProcess helmProcess) : base(helmProcess)
    { }
    public void Install()
    {
        var configDirectory = Path.Combine("Charts", "PgAdmin", "config.yaml");

        base.InstallFromRepo("runix/pgadmin4",
            "runix",
            "https://helm.runix.net",
            "pgadmin",
            configDirectory);
    }

    public string[] Parameters => new[] { "--pg-admin", "-pssql-admin" };
    public string Description => "Install pgadmin chart on kind";
    public ExtraPortMapping[] GetPortMapping() => Ports.PgAdmin;
}
