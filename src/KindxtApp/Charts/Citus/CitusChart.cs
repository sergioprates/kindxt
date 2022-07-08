using Kindxt.Kind;

namespace Kindxt.Charts.Citus
{
    public class CitusChart : HelmChartBase, IHelmChart
    {
        public void Install()
        {
            //var chartPath = Path.Combine("Charts", "Citus", "chart");

            //InstallFromLocalChart(chartPath,
            //    "citus");
        }

        public string[] Parameters => new[] {"--citus"};
        public string Description => "Install citus chart on kind";
        public ExtraPortMapping GetPortMapping() => Ports.Postgres;
    }
}
