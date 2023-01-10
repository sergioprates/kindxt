using FakeItEasy;
using FluentAssertions;
using Kindxt.Charts.Istio;
using Kindxt.Extensions;
using Kindxt.Kind;
using Kindxt.Processes;

namespace UnitTests.Charts.Istio;

public class IstioChartTests : TestBase
{
    [SetUp]
    public void SetUp()
    {
        AutoFake.Provide(A.Fake<HelmProcess>());
        AutoFake.Provide(A.Fake<KubectlProcess>());

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(A<string>.Ignored,
                A<string>.Ignored, A<bool>.Ignored, A<int>.Ignored))
            .Returns(AutoFake.Resolve<HelmProcess>());

        A.CallTo(() => AutoFake.Resolve<KubectlProcess>().ExecuteCommand(A<string>.Ignored,
                A<string>.Ignored, A<bool>.Ignored, A<int>.Ignored))
            .Returns(AutoFake.Resolve<KubectlProcess>());
    }

    [Test]
    public void ParametersShouldBeCorrect()
    {
        AutoFake.Resolve<IstioChart>().Parameters
            .Should().BeEquivalentTo(new string[] { "--istio-ingress" });
    }
    [Test]
    public void DescriptionShouldBeCorrect()
    {
        AutoFake.Resolve<IstioChart>().Description
            .Should().Be("Install istio-ingress chart on kind");
    }

    [Test]
    public void GetPortMappingShouldReturnCorrectMap()
    {
        AutoFake.Resolve<IstioChart>().GetPortMapping()
            .Should().BeEquivalentTo(new ExtraPortMapping[]
            {
                    new()
                    {
                        ContainerPort = 30004,
                        HostPort = 8081,
                        Protocol = "TCP"
                    },
                    new()
                    {
                        ContainerPort = 30005,
                        HostPort = 8082,
                        Protocol = "TCP"
                    },
                    new()
                    {
                        ContainerPort = 30006,
                        HostPort = 8083,
                        Protocol = "TCP"
                    },
            });
    }

    [Test]
    public void ShouldCreateNamespaceIstioSystem()
    {
        AutoFake.Resolve<IstioChart>().Install();

        A.CallTo(() =>
                AutoFake.Resolve<KubectlProcess>().ExecuteCommand(
                    "create namespace istio-system", A<string>.Ignored, true, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }

    [Test]
    public void ShouldLabelNamespaceIstioSystemWithIstioInjection()
    {
        AutoFake.Resolve<IstioChart>().Install();

        A.CallTo(() =>
                AutoFake.Resolve<KubectlProcess>().ExecuteCommand(
                    "label namespace istio-system istio-injection=enabled --overwrite", A<string>.Ignored, true, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }

    [Test]
    public void ShouldCreateDefaultGatewayOnK8s()
    {
        var configDirectory = Path.Combine("Charts", "Istio");

        AutoFake.Resolve<IstioChart>().Install();

        A.CallTo(() =>
                AutoFake.Resolve<KubectlProcess>().ExecuteCommand(
                    "apply -f gateway.yaml", Path.Combine(KindxtPath.GetProcessPath(), configDirectory), false, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }
}
