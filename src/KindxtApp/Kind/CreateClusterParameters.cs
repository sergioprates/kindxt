namespace Kindxt.Kind;
public class CreateClusterParameters
{
    public string KindImage { get; set; }
    public IEnumerable<string> ChartParameters { get; set; }
}
