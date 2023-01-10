using System.Text;

namespace Kindxt.Managers;

public class FileManager
{
    public virtual void CreateDirectoryIfNotExists(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }
    public virtual void WriteFile(string path, string content)
    {
        File.WriteAllText(path, content, Encoding.UTF8);
    }
    public virtual TextReader GetReader(string path)
    {
        return new StreamReader(path);
    }
}
