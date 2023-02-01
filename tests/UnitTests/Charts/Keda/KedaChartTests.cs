using Autofac.Extras.FakeItEasy;
using FakeItEasy;
using FluentAssertions;
using Kindxt.Charts.Keda;
using Kindxt.Extensions;
using Kindxt.Kind;
using Kindxt.Processes;

namespace UnitTests.Charts.Keda;

public class KedaChartTests : TestBase
{
    private const string ChartName = "kedacore/keda";
    private const string RepoName = "kedacore";
    private const string RepoUrl = "https://kedacore.github.io/charts";
    private const string ReleaseName = "keda";
    private const string Namespace = "keda";

    [SetUp]
    public void SetUp()
    {
        AutoFake.Provide(A.Fake<HelmProcess>());
        AutoFake.Provide(A.Fake<KubectlProcess>());
        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(A<string>.Ignored,
                A<string>.Ignored, A<bool>.Ignored, A<int>.Ignored))
            .Returns(AutoFake.Resolve<HelmProcess>());
    }

    [Test]
    public void ParametersShouldBeCorrect()
    {
        AutoFake.Resolve<KedaChart>().Parameters
            .Should().BeEquivalentTo(new string[] { "--keda" });
    }

    [Test]
    public void DescriptionShouldBeCorrect()
    {
        AutoFake.Resolve<KedaChart>().Description
            .Should().Be("Install Keda chart on kind");
    }

    [Test]
    public void GetPortMappingShouldReturnCorrectMap()
    {
        AutoFake.Resolve<KedaChart>().GetPortMapping()
            .Should().BeEquivalentTo(Array.Empty<ExtraPortMapping>());
    }

    [Test]
    public void InstallShouldExecuteCommandRepoAdd()
    {
        AutoFake.Resolve<KedaChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                $"repo add {RepoName} {RepoUrl}", A<string>.Ignored,
                true, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }

    [Test]
    public void InstallShouldExecuteCommandRepoUpdate()
    {
        AutoFake.Resolve<KedaChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                "repo update", A<string>.Ignored,
                true, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }

    [Test]
    public void InstallShouldExecuteCommandUninstall()
    {
        AutoFake.Resolve<KedaChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                $"uninstall {ReleaseName} -n {Namespace}", A<string>.Ignored,
                true, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }

    [Test]
    public void InstallShouldExecuteCommandInstall()
    {
        AutoFake.Resolve<KedaChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                $"install {ReleaseName} {ChartName} -n {Namespace} --wait --debug --timeout=5m",
                KindxtPath.GetProcessPath(),
                false, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }
}
