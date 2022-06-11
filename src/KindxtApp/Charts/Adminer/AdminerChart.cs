namespace KindxtApp.Charts.Adminer
{
    public class AdminerChart : HelmChartBase, IHelmChart
    {
        public void Install()
        {
            var configDirectory = Path.Combine("Charts", "Adminer");

            base.Install("cetic/adminer",
                "cetic",
                "https://cetic.github.io/helm-charts",
                "adminer",
                configDirectory);
        }
    }
}
