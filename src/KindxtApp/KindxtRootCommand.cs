using System.CommandLine;
using System.CommandLine.Invocation;
using Kindxt.Kind;

namespace Kindxt
{
    public class KindxtRootCommand : RootCommand
    {
        public KindxtRootCommand()
            : base("Kindxt is a extension from kind")
        { }

        public void RegisterHandler(KindClusterBuilder kindClusterBuilder)
        {
            this.SetHandler<List<string>, InvocationContext>((parameters,
                ctx) =>
            {
                kindClusterBuilder.Build(parameters);
            }, new ParametersBinder(this));
        }
    }
}
