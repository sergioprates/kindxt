using Kindxt.Commands;
using Kindxt.Extensions;
using Kindxt.Kind;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

var kindxtCommand = new KindxtRootCommand();

var services = new ServiceCollection().RegisterDependencies();

var serviceProvider = services.BuildServiceProvider();

var kindClusterBuilder = serviceProvider.GetRequiredService<KindClusterBuilder>();

kindxtCommand.RegisterHandler(kindClusterBuilder);

if (args.Length == 0)
    args = new string[] { "--help" };

await new CommandLineBuilder(kindxtCommand)
    .UseDefaults()
    .Build()
    .InvokeAsync(args);
