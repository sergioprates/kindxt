using System.CommandLine;
using System.CommandLine.Invocation;
using Kindxt.Kind;
using Microsoft.Extensions.DependencyInjection;

namespace Kindxt.Commands;

public class KindxtRootCommand : RootCommand
{
    public KindxtRootCommand(IServiceProvider serviceProvider)
        : base("Kindxt is a extension from kind")
    {
        this.SetHandler<List<string>, InvocationContext>((parameters,
            ctx) =>
        {
            serviceProvider.GetRequiredService<KindClusterBuilder>().Build(parameters);
        }, new ParametersBinder(this, serviceProvider));
    }
}
