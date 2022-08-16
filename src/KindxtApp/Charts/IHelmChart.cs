using Kindxt.Kind;

namespace Kindxt.Charts
{
    public interface IHelmChart
    {
        void Install();
        string[] Parameters { get; }
        string Description { get; }
        ExtraPortMapping[] GetPortMapping();
    }
}
