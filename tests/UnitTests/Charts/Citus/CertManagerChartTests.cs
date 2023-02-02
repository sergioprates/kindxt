using FakeItEasy;
using Kindxt.Charts.Citus;
using Kindxt.Extensions;
using Kindxt.Processes;

namespace UnitTests.Charts.Citus;

internal class CertManagerChartTests : TestBase
{
    private const string ReleaseName = "cert-manager";
    private const string Namespace = "cert-manager";
    private const string ChartName = "jetstack/cert-manager";
    private const string RepoName = "jetstack";
    private const string RepoUrl = "https://charts.jetstack.io";

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
        AutoFake.Resolve<CitusChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                $"repo add {RepoName} {RepoUrl}", A<string>.Ignored,
                true, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }
    [Test]
    public void InstallShouldExecuteCommandUninstall()
    {
        AutoFake.Resolve<CitusChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                $"uninstall {ReleaseName} -n {Namespace}", A<string>.Ignored,
                true, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }
    [Test]
    public void InstallShouldExecuteCommandInstall()
    {
        AutoFake.Resolve<CitusChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                $"install {ReleaseName} {ChartName} -n {Namespace} --wait --debug --timeout=5m  --version v1.5.4 --set installCRDs=true",
                KindxtPath.GetProcessPath(),
                false, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }
}
