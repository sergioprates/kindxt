namespace Kindxt.Kind;
public class CreateClusterParameters
{
    public string KindImage { get; set; }
    public IReadOnlyList<string> ChartParameters { get; set; }
    public IReadOnlyList<string> ChartsToInstall { get; set; }
}
