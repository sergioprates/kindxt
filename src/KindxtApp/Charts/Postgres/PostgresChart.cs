namespace KindxtApp.Charts.Postgres
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
    }
}
