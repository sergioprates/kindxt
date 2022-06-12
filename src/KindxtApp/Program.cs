using KindxtApp;
using KindxtApp.Kind;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

var kindxtCommand = new KindxtRootCommand();

var kindClusterBuilder = new KindClusterBuilder();

kindxtCommand.RegisterHandler(kindClusterBuilder);

await new CommandLineBuilder(kindxtCommand)
    .UseDefaults()
    .Build()
    .InvokeAsync(args);

kindClusterBuilder.Build();
