using Kindxt.Commands;
using System.CommandLine;
using System.CommandLine.Binding;

namespace Kindxt.Kind;
public class CreateClusterBinder : BinderBase<CreateClusterParameters>
{
    private readonly CreateClusterCommand _command;
    private readonly Option<string> _kindImageOption;
    private readonly Option<IEnumerable<string>> _setOption;

    public CreateClusterBinder(CreateClusterCommand command)
    {
        _command = command;

        _kindImageOption = new Option<string>(new[] {"--kind-image"},
            "Use the images from https://hub.docker.com/r/kindest/node/tags")
        {
            IsRequired = false, Arity = ArgumentArity.ExactlyOne
        };

        _command.AddOption(_kindImageOption);

        _setOption = new Option<IEnumerable<string>>("--set")
            { AllowMultipleArgumentsPerToken = true };

        _command.AddOption(_setOption);
    }

    protected override CreateClusterParameters GetBoundValue(BindingContext bindingContext) =>
        new CreateClusterParameters
        {
            KindImage = bindingContext.ParseResult.GetValueForOption(_kindImageOption),
            ChartParameters = bindingContext.ParseResult.GetValueForOption(_setOption),
        };
}
