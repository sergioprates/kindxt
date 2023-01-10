using Kindxt.Kind;

namespace Kindxt.Commands;

public interface ICommandParameter
{
    List<string> Parameters { get; set; }
    void Execute();
    ExtraPortMapping GetPortMapping();
}
