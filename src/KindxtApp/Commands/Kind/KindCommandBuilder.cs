using System.Text;
using KindxtApp.Charts;
using KindxtApp.Charts.Adminer;
using KindxtApp.Charts.NginxIngress;
using KindxtApp.Charts.Postgres;
using KindxtApp.Charts.SqlServer;
using YamlDotNet.Serialization;

namespace KindxtApp.Commands.Kind;
public class KindCommandBuilder
{
    private readonly ISerializer _serializerYaml;
    private readonly KindConfig _kindConfig;
    private bool _createCluster = false;
    private readonly string command = "kind";
    private readonly List<IHelmChart> _charts;

    public KindCommandBuilder(IDeserializer deserializerYaml, ISerializer serializerYaml)
    {
        _serializerYaml = serializerYaml;
        _charts = new List<IHelmChart>();
        _kindConfig = deserializerYaml.Deserialize<KindConfig>(new StreamReader(Path.Combine("Commands", "Kind", "config.yaml")));
    }

    public KindCommandBuilder CreateCluster()
    {
        _createCluster = true;
        return this;
    }

    public void Build()
    {
        var kindConfigFileName = "kind-config.yaml";
        var tmpDirectory = Path.Combine("tmp", "kind");

        if (!Directory.Exists(tmpDirectory))
            Directory.CreateDirectory(tmpDirectory);

        var configText = _serializerYaml.Serialize(_kindConfig);
        File.WriteAllText(Path.Combine(tmpDirectory, kindConfigFileName), configText, Encoding.UTF8);

        var kind = new ProcessWrapper(command);
        if (_createCluster)
        {
            kind
            .ExecuteCommand("delete cluster", ignoreError: true)
            .ExecuteCommand($"create cluster --config={kindConfigFileName} -v 1", tmpDirectory);
        }

        foreach (var chart in _charts)
            chart.Install();
    }

    public KindCommandBuilder WithSqlServer()
    {
        _kindConfig
            .GetNode()
            .ExtraPortMappings
            .Add(Ports.SqlServer);

        _charts.Add(new SqlServerChart());
        return this;
    }
    public KindCommandBuilder WithPostgres()
    {
        _kindConfig
            .GetNode()
            .ExtraPortMappings
            .Add(Ports.Postgres);

        _charts.Add(new PostgresChart());
        return this;
    }
    public KindCommandBuilder WithPgAdmin()
    {
        _kindConfig
            .GetNode()
            .ExtraPortMappings
            .Add(Ports.PgAdmin);

        _charts.Add(new PgAdminChart());
        return this;
    }
    public KindCommandBuilder WithNginxIngress()
    {
        _kindConfig
            .GetNode()
            .ExtraPortMappings
            .Add(Ports.NginxIngress);

        _charts.Add(new NginxIngressChart());
        return this;
    }
}
