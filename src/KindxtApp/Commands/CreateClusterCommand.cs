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
            serviceProvider.GetRequiredService<KindClusterBuilder>().Build(parameters);
        }, new CreateClusterBinder(this, serviceProvider));
    }
}
