using System.Text;
using Kindxt.Charts;
using Kindxt.Charts.Adminer;
using Kindxt.Charts.NginxIngress;
using Kindxt.Charts.Postgres;
using Kindxt.Charts.SqlServer;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Kindxt.Kind;
public class KindClusterBuilder
{
    private readonly ISerializer _serializerYaml;
    private readonly KindConfig _kindConfig;
    private bool _createCluster = false;
    private readonly string command = "kind";
    private readonly List<IHelmChart> _charts;

    public KindClusterBuilder()
    {
        var deserializerYaml = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        _serializerYaml = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        _charts = new List<IHelmChart>();
        _kindConfig = deserializerYaml.Deserialize<KindConfig>(
            new StreamReader(Path.Combine(Path.GetDirectoryName(Environment.ProcessPath!)!, "Kind", "config.yaml")));
    }

    public KindClusterBuilder CreateCluster()
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

    public KindClusterBuilder WithSqlServer()
    {
        _kindConfig
            .GetNode()
            .ExtraPortMappings
            .Add(Ports.SqlServer);

        _charts.Add(new SqlServerChart());
        return this;
    }
    public KindClusterBuilder WithPostgres()
    {
        _kindConfig
            .GetNode()
            .ExtraPortMappings
            .Add(Ports.Postgres);

        _charts.Add(new PostgresChart());
        return this;
    }
    public KindClusterBuilder WithPgAdmin()
    {
        _kindConfig
            .GetNode()
            .ExtraPortMappings
            .Add(Ports.PgAdmin);

        _charts.Add(new PgAdminChart());
        return this;
    }
    public KindClusterBuilder WithNginxIngress()
    {
        _kindConfig
            .GetNode()
            .ExtraPortMappings
            .Add(Ports.NginxIngress);

        _charts.Add(new NginxIngressChart());
        return this;
    }
}
