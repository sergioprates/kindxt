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
var adminerOption = new Option<bool>(new[] { "--adminer", "-pssql-admin" }, "Install adminer chart on kind")
{
    IsRequired = false,
    Arity = ArgumentArity.Zero
};
rootCommand.SetHandler<bool, bool, bool, bool, InvocationContext>((createCluster,
    installSqlServer,
    installPostgres,
    installAdminer,
    ctx) =>
{
    if (createCluster)
        kindCommandBuilder.CreateCluster();
    if (installSqlServer)
        kindCommandBuilder.WithSqlServer();
    if (installPostgres)
        kindCommandBuilder.WithPostgres();
    if (installAdminer)
        kindCommandBuilder.WithAdminer();
}, createClusterOption, sqlServerOption, postgresOption, adminerOption);

rootCommand.AddOption(createClusterOption);
rootCommand.AddOption(sqlServerOption);
rootCommand.AddOption(postgresOption);
rootCommand.AddOption(adminerOption);

var commandBuilder = new CommandLineBuilder(rootCommand).UseDefaults();

var command = commandBuilder.Build();
await command.InvokeAsync(args);

kindCommandBuilder.Build();
