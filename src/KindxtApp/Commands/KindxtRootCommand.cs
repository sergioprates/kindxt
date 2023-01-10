using System.CommandLine;
using System.CommandLine.Invocation;
using Kindxt.Kind;

namespace Kindxt.Commands;

public class KindxtRootCommand : RootCommand
{
    private readonly IServiceProvider _serviceProvider;

    public KindxtRootCommand(IServiceProvider serviceProvider)
        : base("Kindxt is a extension from kind") =>
        _serviceProvider = serviceProvider;

    public void RegisterHandler(KindClusterBuilder kindClusterBuilder)
    {
        this.SetHandler<List<string>, InvocationContext>((parameters,
            ctx) =>
        {
            kindClusterBuilder.Build(parameters);
        }, new ParametersBinder(this, _serviceProvider));
    }
}
