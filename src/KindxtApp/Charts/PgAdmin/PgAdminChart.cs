namespace KindxtApp.Charts.Adminer
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
    }
}
