using Autofac.Extras.FakeItEasy;
using FakeItEasy;
using FluentAssertions;
using Kindxt.Charts.RabbitMq;
using Kindxt.Extensions;
using Kindxt.Kind;
using Kindxt.Processes;

namespace UnitTests.Charts.RabbitMq;

public class RabbitMqChartTests : TestBase
{
    private const string ReleaseName = "rabbitmq";
    private const string Namespace = "default";
    private const string ChartName = "bitnami/rabbitmq";
    private const string RepoName = "bitnami";
    private const string RepoUrl = "https://charts.bitnami.com/bitnami";
    private string _pathConfig = Path.Combine("Charts", "RabbitMq", "config.yaml");

    [SetUp]
    public void SetUp()
    {
        AutoFake.Provide(A.Fake<HelmProcess>());
        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(A<string>.Ignored,
                A<string>.Ignored, A<bool>.Ignored, A<int>.Ignored))
            .Returns(AutoFake.Resolve<HelmProcess>());
    }

    [Test]
    public void ParametersShouldBeCorrect()
    {
        AutoFake.Resolve<RabbitMqChart>().Parameters
            .Should().BeEquivalentTo(new string[] { "--rabbitmq" });
    }

    [Test]
    public void DescriptionShouldBeCorrect()
    {
        AutoFake.Resolve<RabbitMqChart>().Description
            .Should().Be("Install RabbitMQ chart on kind");
    }

    [Test]
    public void GetPortMappingShouldReturnCorrectMap()
    {
        AutoFake.Resolve<RabbitMqChart>().GetPortMapping()
            .Should().BeEquivalentTo(new ExtraPortMapping[]
            {
                    new()
                    {
                        ContainerPort = 30009,
                        HostPort = 5672,
                        Protocol = "TCP"
                    },
                    new()
                    {
                        ContainerPort = 30010,
                        HostPort = 15672,
                        Protocol = "TCP"
                    }
            });
    }

    [Test]
    public void InstallShouldExecuteCommandRepoAdd()
    {
        AutoFake.Resolve<RabbitMqChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                $"repo add {RepoName} {RepoUrl}", A<string>.Ignored,
                true, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }

    [Test]
    public void InstallShouldExecuteCommandRepoUpdate()
    {
        AutoFake.Resolve<RabbitMqChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                "repo update", A<string>.Ignored,
                true, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }

    [Test]
    public void InstallShouldExecuteCommandUninstall()
    {
        AutoFake.Resolve<RabbitMqChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                $"uninstall {ReleaseName} -n {Namespace}", A<string>.Ignored,
                true, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }

    [Test]
    public void InstallShouldExecuteCommandInstall()
    {
        AutoFake.Resolve<RabbitMqChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommandWithWait(
                $"install {ReleaseName} {ChartName} -n {Namespace} --wait --debug --timeout=5m -f {_pathConfig}",
                KindxtPath.GetProcessPath(),
                false, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }
}
