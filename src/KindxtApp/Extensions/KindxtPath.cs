namespace Kindxt.Extensions;

public static class KindxtPath
{
    public static string GetProcessPath() => Path.GetDirectoryName(Environment.ProcessPath!)!;
}
