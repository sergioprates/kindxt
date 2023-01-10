using FakeItEasy;
using Kindxt.Charts.Istio;
using Kindxt.Extensions;
using Kindxt.Processes;

namespace UnitTests.Charts.Istio;

public class IstioBaseTests : TestBase
{
    private const string ReleaseName = "istio-base";
    private const string Namespace = "istio-system";
    private const string ChartName = "istio/base";
    private const string RepoName = "istio";
    private const string RepoUrl = "https://istio-release.storage.googleapis.com/charts";

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
    public void InstallShouldExecuteCommandRepoAdd()
    {
        AutoFake.Resolve<IstioChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                $"repo add {RepoName} {RepoUrl}", A<string>.Ignored,
                true, A<int>.Ignored))
            .MustHaveHappened();
    }
    [Test]
    public void InstallShouldExecuteCommandRepoUpdate()
    {
        AutoFake.Resolve<IstioChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                "repo update", A<string>.Ignored,
                true, A<int>.Ignored))
            .MustHaveHappened();
    }
    [Test]
    public void InstallShouldExecuteCommandUninstall()
    {
        AutoFake.Resolve<IstioChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                $"uninstall {ReleaseName} -n {Namespace}", A<string>.Ignored,
                true, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }
    [Test]
    public void InstallShouldExecuteCommandInstall()
    {
        AutoFake.Resolve<IstioChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                $"install {ReleaseName} {ChartName} -n {Namespace} --wait --debug --timeout=5m",
                KindxtPath.GetProcessPath(),
                false, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }
}
