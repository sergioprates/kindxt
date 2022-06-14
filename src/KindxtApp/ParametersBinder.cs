using System.CommandLine;
using System.CommandLine.Binding;

namespace Kindxt
{
    public class ParametersBinder : BinderBase<KindxtParameters>
    {
        private readonly Option<bool> _sqlServer;
        private readonly Option<bool> _postgres;
        private readonly Option<bool> _pgadmin;
        private readonly Option<bool> _nginxIngress;
        private readonly Option<bool> _createCluster;
        private readonly KindxtRootCommand _kindxtRootCommand;

        public ParametersBinder(KindxtRootCommand kindxtRootCommand)
        {
            _kindxtRootCommand = kindxtRootCommand;
            _createCluster = AddBoolOption(new[] { "--create-cluster", "-c" },
                "Delete the cluster and create a new one");

            _sqlServer = AddBoolOption(new[] { "--sqlserver", "-sql" },
                "Install sqlserver chart on kind");

            _postgres = AddBoolOption(new[] { "--postgres", "-pssql" },
                "Install postgres chart on kind");

            _pgadmin = AddBoolOption(new[] { "--pgAdmin", "-pssql-admin" },
                "Install pgadmin chart on kind");

            _nginxIngress = AddBoolOption(new[] { "--nginx-ingress", "-nginx" },
                "Install nginx-ingress chart on kind");
        }

        protected override KindxtParameters GetBoundValue(BindingContext bindingContext)
        {
            var result = bindingContext.ParseResult;
            return new KindxtParameters(
                result.GetValueForOption(_createCluster),
                result.GetValueForOption(_sqlServer),
                result.GetValueForOption(_postgres),
                result.GetValueForOption(_pgadmin),
                result.GetValueForOption(_nginxIngress)
                );
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
    public record KindxtParameters(
        bool CreateCluster,
        bool InstallSqlServer,
        bool InstallPostgres,
        bool InstallPgAdmin,
        bool InstallNginxIngress);
}
