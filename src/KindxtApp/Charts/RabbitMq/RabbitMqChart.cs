using Kindxt.Kind;
using Kindxt.Processes;

namespace Kindxt.Charts.RabbitMq;

public class RabbitMqChart : HelmChartBase, IHelmChart
{
    public RabbitMqChart(HelmProcess helmProcess) : base(helmProcess)
    {
    }

    public void Install()
    {
        var configDirectory = Path.Combine("Charts", "RabbitMq", "config.yaml");

        base.InstallFromRepo("bitnami/rabbitmq",
            "bitnami",
            "https://charts.bitnami.com/bitnami",
            "rabbitmq",
            configDirectory);
    }

    public string[] Parameters => new[] { "--rabbitmq" };
    public string Description => "Install RabbitMQ chart on kind";
    public ExtraPortMapping[] GetPortMapping() => Ports.RabbitMq;
}
