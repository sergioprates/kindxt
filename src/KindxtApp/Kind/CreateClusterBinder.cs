using Kindxt.Charts;
using Kindxt.Commands;
using Kindxt.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using System.CommandLine.Binding;

namespace Kindxt.Kind;
public class CreateClusterBinder : BinderBase<CreateClusterParameters>
{
    private readonly CreateClusterCommand _command;
    private readonly Option<string> _kindImageOption;
    private readonly Option<IEnumerable<string>> _setOption;
    private readonly List<string> _parametersAvailablesForCharts;

    public CreateClusterBinder(CreateClusterCommand command, IServiceProvider serviceProvider)
    {
        _command = command;
        _parametersAvailablesForCharts = new List<string>();

        _kindImageOption = new Option<string>(new[] { "--kind-image" },
            "Use the images from https://hub.docker.com/r/kindest/node/tags")
        {
            IsRequired = false,
            Arity = ArgumentArity.ExactlyOne
        };

        _command.AddOption(_kindImageOption);

        _setOption = new Option<IEnumerable<string>>("--set")
        { AllowMultipleArgumentsPerToken = true };

        _command.AddOption(_setOption);

        var typesOfHelmChartAvailables = TypeExtensions.GetAllTypes<IHelmChart>();

        foreach (var helmChartAvailable in typesOfHelmChartAvailables)
        {
            var helmChart = (IHelmChart)serviceProvider.GetRequiredService(helmChartAvailable);
            AddBoolOption(helmChart.Parameters, helmChart.Description);

            _parametersAvailablesForCharts.AddRange(helmChart.Parameters);
        }
    }

    private Option<bool> AddBoolOption(string[] aliases,
        string description)
    {
        var option = new Option<bool>(aliases,
            description)
        {
            IsRequired = false,
            Arity = ArgumentArity.Zero
        };

        _command.AddOption(option);

        return option;
    }

    protected override CreateClusterParameters GetBoundValue(BindingContext bindingContext) =>
        new CreateClusterParameters
        {
            KindImage = bindingContext.ParseResult.GetValueForOption(_kindImageOption),
            ChartParameters = bindingContext.ParseResult.GetValueForOption(_setOption).ToList(),
            ChartsToInstall = bindingContext.ParseResult.Tokens.Where(x=> _parametersAvailablesForCharts.Contains(x.Value)).Select(x=> x.Value).ToList()
        };
}
