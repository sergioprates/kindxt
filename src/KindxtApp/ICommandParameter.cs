using Kindxt.Kind;

namespace Kindxt
{
    public  interface ICommandParameter
    {
        List<string> Parameters { get; set; }
        void Execute();
        ExtraPortMapping GetPortMapping();
    }
}
