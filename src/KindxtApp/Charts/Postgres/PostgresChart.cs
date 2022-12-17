using Kindxt.Kind;
using Kindxt.Processes;

namespace Kindxt.Charts.Postgres
{
    public class PostgresChart : HelmChartBase, IHelmChart
    {
        public PostgresChart(HelmProcess helmProcess) : base(helmProcess)
        { }
        public void Install()
        {
            var configDirectory = Path.Combine("Charts", "Postgres", "config.yaml");

            base.InstallFromRepo("bitnami/postgresql",
                "bitnami",
                "https://charts.bitnami.com/bitnami",
                "postgres",
                configDirectory);
        }

        public string[] Parameters => new[] { "--postgres", "-pssql" };
        public string Description => "Install postgres chart on kind";
        public ExtraPortMapping[] GetPortMapping() => Ports.Postgres;
    }
}
