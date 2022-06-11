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

var recreateKindOption = new Option<bool>(new[] { "--recreate-cluster", "-r" }, "Delete the cluster and create a new one")
{
    IsRequired = false,
    Arity = ArgumentArity.Zero
};
var sqlServerOption = new Option<bool>(new[] { "--sqlserver", "-sql" }, "Install sql server chart on kind")
{
    IsRequired = false,
    Arity = ArgumentArity.Zero
};
rootCommand.SetHandler<bool, bool, InvocationContext>((recreateCluster,
    installSqlServer,
    ctx) =>
{
    if (recreateCluster)
        kindCommandBuilder.RecreateCluster();
    if (installSqlServer)
        kindCommandBuilder.WithSqlServer();
}, recreateKindOption, sqlServerOption);

rootCommand.AddOption(recreateKindOption);
rootCommand.AddOption(sqlServerOption);

var commandBuilder = new CommandLineBuilder(rootCommand).UseDefaults();


var migrationCommand = commandBuilder.Build();
await migrationCommand.InvokeAsync(args);

kindCommandBuilder.Build();
