using FakeItEasy;
using FluentAssertions;
using Kindxt.Charts.Citus;
using Kindxt.Extensions;
using Kindxt.Kind;
using Kindxt.Processes;

namespace UnitTests.Charts.Citus
{
    public class CitusChartTests : TestBase
    {
        private const string ReleaseName = "citus";
        private const string Namespace = "default";
        private const string ChartName = "prates-charts/citus";
        private const string RepoName = "prates-charts";
        private const string RepoUrl = "https://sergioprates.github.io/prates-charts/";

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
            AutoFake.Resolve<CitusChart>().Parameters
                .Should().BeEquivalentTo(new string[] { "--citus" });
        }
        [Test]
        public void DescriptionShouldBeCorrect()
        {
            AutoFake.Resolve<CitusChart>().Description
                .Should().Be("Install citus chart on kind");
        }

        [Test]
        public void GetPortMappingShouldReturnCorrectMap()
        {
            AutoFake.Resolve<CitusChart>().GetPortMapping()
                .Should().BeEquivalentTo(new ExtraPortMapping[]
                {
                    new()
                    {
                        ContainerPort = 30007,
                        HostPort = 5433,
                        Protocol = "TCP"
                    }
                });
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
        public void InstallShouldExecuteCommandRepoUpdate()
        {
            AutoFake.Resolve<CitusChart>().Install();

            A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                    "repo update", A<string>.Ignored,
                    true, A<int>.Ignored))
                .MustHaveHappenedOnceOrMore();
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
                    $"install {ReleaseName} {ChartName} -n {Namespace} --wait --debug --timeout=5m",
                    KindxtPath.GetProcessPath(),
                    false, A<int>.Ignored))
                .MustHaveHappenedOnceExactly();
        }
    }
}
