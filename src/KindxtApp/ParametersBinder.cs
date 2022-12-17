using System.CommandLine;
using System.CommandLine.Binding;
using Kindxt.Charts;
using Kindxt.Commands;
using Kindxt.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Kindxt
{
    public class ParametersBinder : BinderBase<List<string>>
    {
        private readonly KindxtRootCommand _kindxtRootCommand;
        private readonly IServiceProvider _serviceProvider;

        public ParametersBinder(KindxtRootCommand kindxtRootCommand, IServiceProvider serviceProvider)
        {
            _kindxtRootCommand = kindxtRootCommand;
            _serviceProvider = serviceProvider;
            var helmChartsAvailable = TypeExtensions.GetAllTypes<IHelmChart>();

            AddBoolOption(new[] { "--create-cluster", "-c" },
                "Delete the cluster and create a new one");
            foreach (var helmChartAvailable in helmChartsAvailable)
            {
                var helmChart = (IHelmChart)serviceProvider.GetRequiredService(helmChartAvailable);
                AddBoolOption(helmChart.Parameters, helmChart.Description);
            }
        }

        protected override List<string> GetBoundValue(BindingContext bindingContext)
        {
            var result = bindingContext.ParseResult;
            var parameters = result.Tokens.Select(x => x.Value).ToList();
            return parameters;
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

            _kindxtRootCommand.AddOption(option);

            return option;
        }
    }
}
