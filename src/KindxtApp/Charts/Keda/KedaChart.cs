using Kindxt.Kind;
using Kindxt.Processes;

namespace Kindxt.Charts.Keda;

public class KedaChart : HelmChartBase, IHelmChart
{
    private readonly KubectlProcess _kubectlProcess;
    public KedaChart(HelmProcess helmProcess, KubectlProcess kubectlProcess) : base(helmProcess)
        => _kubectlProcess = kubectlProcess;

    public void Install()
    {
        _kubectlProcess.ExecuteCommand($"create namespace keda", ignoreError: true);

        base.InstallFromRepo("kedacore/keda",
            "kedacore",
            "https://kedacore.github.io/charts",
            "keda",
            @namespace: "keda");
    }

    public string[] Parameters => new[] { "--keda" };
    public string Description => "Install Keda chart on kind";
    public ExtraPortMapping[] GetPortMapping() => Array.Empty<ExtraPortMapping>();
}
