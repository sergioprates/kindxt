using KindxtApp.Kind;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace KindxtApp
{
    public class KindxtRootCommand : RootCommand
    {
        public KindxtRootCommand()
            : base("Kindxt is a extension from kind")
        { }

        public void RegisterHandler(KindClusterBuilder kindClusterBuilder)
        {
            this.SetHandler<KindxtParameters, InvocationContext>((parameters,
                ctx) =>
            {
                if (parameters.CreateCluster)
                    kindClusterBuilder.CreateCluster();
                if (parameters.InstallSqlServer)
                    kindClusterBuilder.WithSqlServer();
                if (parameters.InstallPostgres)
                    kindClusterBuilder.WithPostgres();
                if (parameters.InstallPgAdmin)
                    kindClusterBuilder.WithPgAdmin();
                if (parameters.InstallNginxIngress)
                    kindClusterBuilder.WithNginxIngress();
            }, new ParametersBinder(this));
        }
    }
}
