namespace Kindxt.Charts.NginxIngress
{
    public class NginxIngressChart : HelmChartBase, IHelmChart
    {
        public void Install()
        {
            var configDirectory = Path.Combine("Charts", "NginxIngress");

            base.Install("ingress-nginx/ingress-nginx",
                "ingress-nginx",
                "https://kubernetes.github.io/ingress-nginx",
                "nginx-ingress",
                configDirectory);
        }
    }
}
