using Kindxt.Extensions;
using Kindxt.Kind;

namespace Kindxt.Charts.Istio
{
    public class IstioChart : HelmChartBase, IHelmChart
    {
        public void Install()
        {
            var configDirectory = Path.Combine("Charts", "Istio");

            var kubectl = new ProcessWrapper("kubectl");
            kubectl
                .ExecuteCommand($"create namespace istio-system", ignoreError: true)
                .ExecuteCommand($"create namespace istio-ingress", ignoreError: true)
                .ExecuteCommand($"label namespace istio-ingress istio-injection=enabled --overwrite",
                    ignoreError: true);

            base.Install("istio/base",
                "istio",
                "https://istio-release.storage.googleapis.com/charts",
                "istio-base",
                @namespace: "istio-system");

            base.Install("istio/istiod",
                "istio",
                "https://istio-release.storage.googleapis.com/charts",
                "istiod",
                @namespace: "istio-system");

            var configFile = Path.Combine(configDirectory, "istio-ingress-config.yaml");
            base.Install("istio/gateway",
                "istio",
                "https://istio-release.storage.googleapis.com/charts",
                "istiod",
                configFile,
                @namespace: "istio-ingress");

            kubectl.ExecuteCommand("apply -f gateway.yaml", Path.Combine(KindxtPath.GetProcessPath(), configDirectory));
        }

        public string[] Parameters => new[] { "--istio-ingress"};
        public string Description => "Install istio-ingress chart on kind";
        public ExtraPortMapping[] GetPortMapping() => Ports.Istio;
    }
}
