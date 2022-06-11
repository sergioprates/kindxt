using System.Text;
using KindxtApp.Charts;
using KindxtApp.Charts.SqlServer;
using YamlDotNet.Serialization;

namespace KindxtApp.Commands.Kind;
public class KindCommandBuilder
{
    private readonly IDeserializer _deserializerYaml;
    private readonly ISerializer _serializerYaml;
    private readonly KindConfig _kindConfig;
    private bool _recreateCluster = false;
    private readonly string command = "kind";
    private readonly List<IHelmChart> _charts;

    public KindCommandBuilder(IDeserializer deserializerYaml, ISerializer serializerYaml)
    {
        _deserializerYaml = deserializerYaml;
        _serializerYaml = serializerYaml;
        _charts = new List<IHelmChart>();
        _kindConfig = _deserializerYaml.Deserialize<KindConfig>(new StreamReader(Path.Combine("Commands", "Kind", "config.yaml")));
    }

    public KindCommandBuilder WithSqlServer()
    {
        _kindConfig
            .GetNode()
            .ExtraPortMappings
            .Add(Ports.SqlServer);

        _charts.Add(new SqlServerChartBuilder());
        return this;
    }
    public KindCommandBuilder RecreateCluster()
    {
        _recreateCluster = true;
        return this;
    }

    public ProcessWrapper Build()
    {
        var kindConfigFileName = "kind-config.yaml";
        var tmpDirectory = Path.Combine("tmp", "kind");

        if (!Directory.Exists(tmpDirectory))
            Directory.CreateDirectory(tmpDirectory);

        var configText = _serializerYaml.Serialize(_kindConfig);
        File.WriteAllText(Path.Combine(tmpDirectory, kindConfigFileName), configText, Encoding.UTF8);

        if (_recreateCluster)
            new ProcessWrapper(command, "delete cluster").ExecuteCommand();

        var process = new ProcessWrapper(command, $"create cluster --config={kindConfigFileName} -v 1")
            .ExecuteCommand(tmpDirectory);

        foreach (var chart in _charts)
            chart.Install();

        return process;
    }
}
