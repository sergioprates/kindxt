using Kindxt.Kind;

namespace Kindxt.Charts.Postgres
{
    public class PostgresChart : HelmChartBase, IHelmChart
    {
        public void Install()
        {
            var configDirectory = Path.Combine("Charts", "Postgres");

            base.Install("bitnami/postgresql",
                "bitnami",
                "https://charts.bitnami.com/bitnami",
                "postgres",
                configDirectory);
        }

        public string[] Parameters => new[] { "--postgres", "-pssql" };
        public string Description => "Install postgres chart on kind";
        public ExtraPortMapping GetPortMapping() => Ports.Postgres;
    }
}
