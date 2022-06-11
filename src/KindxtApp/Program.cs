using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using KindxtApp.Commands.Kind;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

var deserializer = new DeserializerBuilder()
    .WithNamingConvention(CamelCaseNamingConvention.Instance)
    .Build();
var serializer = new SerializerBuilder()
    .WithNamingConvention(CamelCaseNamingConvention.Instance)
    .Build();
var kindCommandBuilder = new KindCommandBuilder(deserializer, serializer);

var rootCommand = new RootCommand("Kindxt is a extension from kind");

var createClusterOption = new Option<bool>(new[] { "--create-cluster", "-c" }, "Delete the cluster and create a new one")
{
    IsRequired = false,
    Arity = ArgumentArity.Zero
};
var sqlServerOption = new Option<bool>(new[] { "--sqlserver", "-sql" }, "Install sql server chart on kind")
{
    IsRequired = false,
    Arity = ArgumentArity.Zero
};
var postgresOption = new Option<bool>(new[] { "--postgres", "-pssql" }, "Install postgres chart on kind")
{
    IsRequired = false,
    Arity = ArgumentArity.Zero
};
var pgAdminOption = new Option<bool>(new[] { "--pgAdmin", "-pssql-admin" }, "Install pgadmin chart on kind")
{
    IsRequired = false,
    Arity = ArgumentArity.Zero
};
var nginxIngressOption = new Option<bool>(new[] { "--nginx-ingress", "-nginx" }, "Install nginx-ingress chart on kind")
{
    IsRequired = false,
    Arity = ArgumentArity.Zero
};
rootCommand.SetHandler<bool, bool, bool, bool, bool, InvocationContext>((createCluster,
    installSqlServer,
    installPostgres,
    installPgAdmin,
    installNginxIngress,
    ctx) =>
{
    if (createCluster)
        kindCommandBuilder.CreateCluster();
    if (installSqlServer)
        kindCommandBuilder.WithSqlServer();
    if (installPostgres)
        kindCommandBuilder.WithPostgres();
    if (installPgAdmin)
        kindCommandBuilder.WithPgAdmin();
    if (installNginxIngress)
        kindCommandBuilder.WithNginxIngress();
}, createClusterOption, sqlServerOption, postgresOption, pgAdminOption, nginxIngressOption);

rootCommand.AddOption(createClusterOption);
rootCommand.AddOption(sqlServerOption);
rootCommand.AddOption(postgresOption);
rootCommand.AddOption(pgAdminOption);
rootCommand.AddOption(nginxIngressOption);

var commandBuilder = new CommandLineBuilder(rootCommand).UseDefaults();

var command = commandBuilder.Build();
await command.InvokeAsync(args);

kindCommandBuilder.Build();
