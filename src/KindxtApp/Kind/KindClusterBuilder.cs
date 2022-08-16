using Kindxt.Charts;
using Kindxt.Extensions;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Kindxt.Kind;
public class KindClusterBuilder
{
    private readonly ISerializer _serializerYaml;
    private readonly KindConfig _kindConfig;
    private readonly string command = "kind";
    private readonly List<IHelmChart> _helmCharts;

    public KindClusterBuilder()
    {
        var deserializerYaml = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        _serializerYaml = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        _helmCharts = new List<IHelmChart>();
        _kindConfig = deserializerYaml.Deserialize<KindConfig>(
            new StreamReader(Path.Combine(KindxtPath.GetProcessPath(), "Kind", "config.yaml")));
    }

    public void Build(List<string> parameters)
    {
        var kindConfigFileName = "kind-config.yaml";
        var tmpDirectory = Path.Combine(KindxtPath.GetProcessPath(), "tmp", "kind");

        if (!Directory.Exists(tmpDirectory))
            Directory.CreateDirectory(tmpDirectory);

        var configText = _serializerYaml.Serialize(_kindConfig);
        File.WriteAllText(Path.Combine(tmpDirectory, kindConfigFileName), configText, Encoding.UTF8);

        var helmChartsAvailable = TypeExtensions.GetAllTypes<IHelmChart>();

        foreach (var type in helmChartsAvailable)
        {
            var helmChart = (IHelmChart)Activator.CreateInstance(type)!;

            if (parameters.Any(x => helmChart.Parameters.Contains(x)))
            {
                _kindConfig
                    .GetNode()
                    .ExtraPortMappings
                    .AddRange(helmChart.GetPortMapping());
                _helmCharts.Add(helmChart);
            }
        }
        var kind = new ProcessWrapper(command);
        if (parameters.Any(x => new[] { "--create-cluster", "-c" }.Contains(x)))
        {
            kind
            .ExecuteCommand("delete cluster", ignoreError: true)
            .ExecuteCommand($"create cluster --config={kindConfigFileName} -v 1", tmpDirectory);
        }

        foreach (var helmChart in _helmCharts)
            helmChart.Install();
    }
}
