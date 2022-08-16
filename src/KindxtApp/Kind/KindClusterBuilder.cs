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

    public KindClusterBuilder()
    {
        var deserializerYaml = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        _serializerYaml = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        _kindConfig = deserializerYaml.Deserialize<KindConfig>(
            new StreamReader(Path.Combine(KindxtPath.GetProcessPath(), "Kind", "config.yaml")));
    }

    public void Build(List<string> parameters)
    {
        var helmChartsAvailable = TypeExtensions.GetAllTypes<IHelmChart>().Select(type => (IHelmChart)Activator.CreateInstance(type)!);

        var helmChartsToInstall = helmChartsAvailable.Where(helmChart =>
            helmChart.Parameters.Any(parameters.Contains)).ToList();

        foreach (var helmChart in helmChartsToInstall)
        {
            _kindConfig
                .GetNode()
                .ExtraPortMappings
                .AddRange(helmChart.GetPortMapping());
        }

        var kind = new ProcessWrapper(command);
        if (parameters.Any(x => new[] { "--create-cluster", "-c" }.Contains(x)))
        {
            var kindConfigFileName = "kind-config.yaml";
            var tmpDirectory = Path.Combine(KindxtPath.GetProcessPath(), "tmp", "kind");

            if (!Directory.Exists(tmpDirectory))
                Directory.CreateDirectory(tmpDirectory);

            var configText = _serializerYaml.Serialize(_kindConfig);
            File.WriteAllText(Path.Combine(tmpDirectory, kindConfigFileName), configText, Encoding.UTF8);
            kind
            .ExecuteCommand("delete cluster", ignoreError: true)
            .ExecuteCommand($"create cluster --config={kindConfigFileName} -v 1", tmpDirectory);
        }

        foreach (var helmChart in helmChartsToInstall)
            helmChart.Install();
    }
}
