using Kindxt;
using Kindxt.Kind;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

var kindxtCommand = new KindxtRootCommand();

var kindClusterBuilder = new KindClusterBuilder();

kindxtCommand.RegisterHandler(kindClusterBuilder);

if (args.Length == 0)
    args = new string[] { "--help" };

await new CommandLineBuilder(kindxtCommand)
    .UseDefaults()
    .Build()
    .InvokeAsync(args);

kindClusterBuilder.Build();
