using Kindxt.Charts;
using Kindxt.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Kindxt.Managers;

public class HelmChartManager
{
    private readonly IServiceProvider _serviceProvider;

    public HelmChartManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public virtual List<IHelmChart> GetHelmCharts(IReadOnlyList<string> parameters)
    {
        var helmChartsAvailable = TypeExtensions.GetAllTypes<IHelmChart>().Select(type => (IHelmChart)_serviceProvider.GetRequiredService(type));

        return helmChartsAvailable.Where(helmChart =>
            helmChart.Parameters.Any(parameters.Contains)).ToList();
    }
}
