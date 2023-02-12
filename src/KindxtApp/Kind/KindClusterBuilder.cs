using Kindxt.Extensions;
using Kindxt.Managers;
using Kindxt.Processes;
using YamlDotNet.Serialization;

namespace Kindxt.Kind;

public class KindClusterBuilder
{
    private readonly ISerializer _serializerYaml;
    private readonly KindProcess _kindProcess;
    private readonly FileManager _fileManager;
    private readonly HelmChartManager _helmChartManager;
    private readonly KindConfig _kindConfig;

    private const string kindConfigFileName = "kind-config.yaml";

    public KindClusterBuilder(ISerializer serializer,
        IDeserializer deserializer,
        KindProcess kindProcess,
        FileManager fileManager,
        HelmChartManager helmChartManager)
    {
        _serializerYaml = serializer;
        _kindProcess = kindProcess;
        _fileManager = fileManager;
        _helmChartManager = helmChartManager;
        _kindConfig = deserializer
            .Deserialize<KindConfig>(
                _fileManager.GetReader(Path.Combine(KindxtPath.GetProcessPath(), "Kind",
                    "config.yaml")));
    }

    public void Build(CreateClusterParameters parameters)
    {
        if (!string.IsNullOrWhiteSpace(parameters.KindImage))
            _kindConfig.Nodes.First().Image = parameters.KindImage;

        var helmChartsToInstall = _helmChartManager.GetHelmCharts(parameters.ChartsToInstall);

        foreach (var helmChart in helmChartsToInstall)
            _kindConfig.AddPortsRange(helmChart.GetPortMapping());

        var tmpDirectory = Path.Combine(KindxtPath.GetProcessPath(), "tmp", "kind");

        _fileManager.CreateDirectoryIfNotExists(tmpDirectory);

        var configText = _serializerYaml.Serialize(_kindConfig);

        _fileManager.WriteFile(Path.Combine(tmpDirectory, kindConfigFileName), configText);

        _kindProcess
            .ExecuteCommand("delete cluster", ignoreError: true)
        .ExecuteCommand($"create cluster --config={kindConfigFileName} -v 1", tmpDirectory);

        foreach (var helmChart in helmChartsToInstall)
            helmChart.Install();
    }
}
