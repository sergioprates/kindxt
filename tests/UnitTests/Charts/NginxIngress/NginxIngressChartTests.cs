using FakeItEasy;
using FluentAssertions;
using Kindxt.Charts.NginxIngress;
using Kindxt.Extensions;
using Kindxt.Kind;
using Kindxt.Processes;

namespace UnitTests.Charts.NginxIngress;

public class NginxIngressChartTests : TestBase
{
    private const string ReleaseName = "nginx-ingress";
    private const string Namespace = "default";
    private const string ChartName = "ingress-nginx/ingress-nginx";
    private const string RepoName = "ingress-nginx";
    private const string RepoUrl = "https://kubernetes.github.io/ingress-nginx";
    private string _pathConfig = Path.Combine("Charts", "NginxIngress", "config.yaml");

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
        AutoFake.Resolve<NginxIngressChart>().Parameters
            .Should().BeEquivalentTo(new string[] { "--nginx-ingress", "-nginx" });
    }
    [Test]
    public void DescriptionShouldBeCorrect()
    {
        AutoFake.Resolve<NginxIngressChart>().Description
            .Should().Be("Install nginx-ingress chart on kind");
    }

    [Test]
    public void GetPortMappingShouldReturnCorrectMap()
    {
        AutoFake.Resolve<NginxIngressChart>().GetPortMapping()
            .Should().BeEquivalentTo(new ExtraPortMapping[]
            {
                new()
                {
                    ContainerPort = 30003,
                    HostPort = 8080,
                    Protocol = "TCP"
                }
            });
    }

    [Test]
    public void InstallShouldExecuteCommandRepoAdd()
    {
        AutoFake.Resolve<NginxIngressChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                $"repo add {RepoName} {RepoUrl}", A<string>.Ignored,
                true, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }
    [Test]
    public void InstallShouldExecuteCommandRepoUpdate()
    {
        AutoFake.Resolve<NginxIngressChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                "repo update", A<string>.Ignored,
                true, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }
    [Test]
    public void InstallShouldExecuteCommandUninstall()
    {
        AutoFake.Resolve<NginxIngressChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                $"uninstall {ReleaseName} -n {Namespace}", A<string>.Ignored,
                true, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }
    [Test]
    public void InstallShouldExecuteCommandInstall()
    {
        AutoFake.Resolve<NginxIngressChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommandWithWait(
                $"install {ReleaseName} {ChartName} -n {Namespace} --wait --debug --timeout=5m -f {_pathConfig}",
                KindxtPath.GetProcessPath(),
                false, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }
}
