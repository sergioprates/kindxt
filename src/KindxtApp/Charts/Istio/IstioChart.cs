using Kindxt.Extensions;
using Kindxt.Kind;
using Kindxt.Processes;

namespace Kindxt.Charts.Istio;

public class IstioChart : HelmChartBase, IHelmChart
{
    private readonly KubectlProcess _kubectlProcess;

    public IstioChart(HelmProcess helmProcess, KubectlProcess kubectlProcess) : base(helmProcess)
    {
        _kubectlProcess = kubectlProcess;
    }
    public void Install()
    {
        var configDirectory = Path.Combine("Charts", "Istio");

        _kubectlProcess
            .ExecuteCommand($"create namespace istio-system", ignoreError: true)
            .ExecuteCommand($"label namespace istio-system istio-injection=enabled --overwrite",
                ignoreError: true);

        base.InstallFromRepo("istio/base",
            "istio",
            "https://istio-release.storage.googleapis.com/charts",
            "istio-base",
            @namespace: "istio-system");

        base.InstallFromRepo("istio/istiod",
            "istio",
            "https://istio-release.storage.googleapis.com/charts",
            "istiod",
            Path.Combine(configDirectory, "istiod-config.yaml"),
            @namespace: "istio-system");

        var configFile = Path.Combine(configDirectory, "istio-ingress-config.yaml");
        base.InstallFromRepo("istio/gateway",
            "istio",
            "https://istio-release.storage.googleapis.com/charts",
            "istio-ingressgateway",
            configFile,
            @namespace: "istio-system");

        _kubectlProcess.ExecuteCommand("apply -f gateway.yaml", Path.Combine(KindxtPath.GetProcessPath(), configDirectory));
    }

    public string[] Parameters => new[] { "--istio-ingress"};
    public string Description => "Install istio-ingress chart on kind";
    public ExtraPortMapping[] GetPortMapping() => Ports.Istio;
}
