using Kindxt.Kind;
using Kindxt.Processes;

namespace Kindxt.Charts.Citus
{
    public class CitusChart : HelmChartBase, IHelmChart
    {
        private readonly KubectlProcess _kubectlProcess;

        public CitusChart(HelmProcess helmProcess, KubectlProcess kubectlProcess) : base(helmProcess)
        {
            _kubectlProcess = kubectlProcess;
        }
        public void Install()
        {
            _kubectlProcess
                .ExecuteCommand($"create namespace cert-manager", ignoreError: true);

            base.InstallFromRepo("jetstack/cert-manager",
                "jetstack",
                "https://charts.jetstack.io",
                "cert-manager",
                @namespace: "cert-manager",
                parameters: "--version v1.5.4 --set installCRDs=true");

            base.InstallFromRepo("prates-charts/citus",
                "prates-charts",
                "https://sergioprates.github.io/prates-charts/",
                "citus");
        }

        public string[] Parameters => new[] { "--citus" };
        public string Description => "Install citus chart on kind";
        public ExtraPortMapping[] GetPortMapping() => Ports.Citus;
    }
}
