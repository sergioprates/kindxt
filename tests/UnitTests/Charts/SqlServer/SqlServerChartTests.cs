using FluentAssertions;
using Kindxt.Charts.SqlServer;
using Kindxt.Kind;

namespace UnitTests.Charts.SqlServer
{
    public class SqlServerChartTests
    {
        [Test]
        public void ParametersShouldBeCorrect()
        {
            var chart = new SqlServerChart();

            chart.Parameters
                .Should().BeEquivalentTo(new string[] { "--sqlserver", "-sql" });
        }
        [Test]
        public void DescriptionShouldBeCorrect()
        {
            var chart = new SqlServerChart();

            chart.Description
                .Should().Be("Install sqlserver chart on kind");
        }

        [Test]
        public void GetPortMappingShouldReturnCorrectMap()
        {
            var chart = new SqlServerChart();

            chart.GetPortMapping()
                .Should().BeEquivalentTo(new ExtraPortMapping[]
                {
                    new()
                    {
                        ContainerPort = 30000,
                        HostPort = 1433,
                        Protocol = "TCP"
                    }
                });
        }
    }
}
