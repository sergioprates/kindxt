using Kindxt.Kind;

namespace Kindxt.Charts.Adminer
{
    public class PgAdminChart : HelmChartBase, IHelmChart
    {
        public void Install()
        {
            var configDirectory = Path.Combine("Charts", "PgAdmin");

            base.Install("runix/pgadmin4",
                "runix",
                "https://helm.runix.net",
                "pgadmin",
                configDirectory);
        }

        public string[] Parameters => new[] { "--pgAdmin", "-pssql-admin" };
        public string Description => "Install pgadmin chart on kind";
        public ExtraPortMapping GetPortMapping() => Ports.PgAdmin;
    }
}
