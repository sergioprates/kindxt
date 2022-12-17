using Kindxt.Kind;
using Kindxt.Processes;

namespace Kindxt.Charts.NginxIngress
{
    public class NginxIngressChart : HelmChartBase, IHelmChart
    {
        public NginxIngressChart(HelmProcess helmProcess) : base(helmProcess)
        { }
        public void Install()
        {
            var configDirectory = Path.Combine("Charts", "NginxIngress", "config.yaml");

            base.InstallFromRepo("ingress-nginx/ingress-nginx",
                "ingress-nginx",
                "https://kubernetes.github.io/ingress-nginx",
                "nginx-ingress",
                configDirectory);
        }

        public string[] Parameters => new[] {"--nginx-ingress", "-nginx"};
        public string Description => "Install nginx-ingress chart on kind";
        public ExtraPortMapping[] GetPortMapping() => Ports.NginxIngress;
    }
}
