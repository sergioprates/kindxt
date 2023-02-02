using Kindxt.Commands;
using Kindxt.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

var services = new ServiceCollection().RegisterDependencies();
var serviceProvider = services.BuildServiceProvider();

var kindxtCommand = new KindxtRootCommand(serviceProvider);

kindxtCommand.AddCommand(new CreateClusterCommand(serviceProvider));

if (args.Length == 0)
    args = new string[] { "--help" };

await new CommandLineBuilder(kindxtCommand)
    .UseDefaults()
    .Build()
    .InvokeAsync(args);
