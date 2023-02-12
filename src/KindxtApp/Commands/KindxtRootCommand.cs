using System.CommandLine;
using System.CommandLine.Invocation;

namespace Kindxt.Commands;

public class KindxtRootCommand : RootCommand
{
    public KindxtRootCommand(IServiceProvider serviceProvider)
        : base("Kindxt is a extension from kind")
    {
        this.SetHandler<List<string>, InvocationContext>((parameters,
            ctx) =>
        {
        }, new ParametersBinder(this, serviceProvider));
    }
}
