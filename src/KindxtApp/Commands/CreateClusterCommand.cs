using Kindxt.Kind;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace Kindxt.Commands;
public class CreateClusterCommand : Command
{
    public CreateClusterCommand(IServiceProvider serviceProvider)
        : base("create", "Create cluster with charts")
    {
        this.SetHandler<CreateClusterParameters, InvocationContext>((parameters,
            ctx) =>
        {
            Console.WriteLine($"kubeversion: {parameters.KindImage}");
            foreach (var parametersChartParameter in parameters.ChartParameters)
            {
                Console.WriteLine(parametersChartParameter);
            }
            foreach (var chartToInstall in parameters.ChartsToInstall)
            {
                Console.WriteLine(chartToInstall);
            }
            serviceProvider.GetRequiredService<KindClusterBuilder>().Build(parameters.ChartsToInstall);
        }, new CreateClusterBinder(this, serviceProvider));
    }
}
