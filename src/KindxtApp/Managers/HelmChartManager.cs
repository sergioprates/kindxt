using Kindxt.Charts;
using Kindxt.Extensions;

namespace Kindxt.Managers
{
    public class HelmChartManager
    {
        public virtual List<IHelmChart> GetHelmCharts(List<string> parameters)
        {
            var helmChartsAvailable = TypeExtensions.GetAllTypes<IHelmChart>().Select(type => (IHelmChart)Activator.CreateInstance(type)!);

            return helmChartsAvailable.Where(helmChart =>
                helmChart.Parameters.Any(parameters.Contains)).ToList();
        }
    }
}
