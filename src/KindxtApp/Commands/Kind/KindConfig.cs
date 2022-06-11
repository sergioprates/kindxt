namespace KindxtApp.Commands.Kind
{
    public class KindConfig
    {
        public KindConfig()
        {
            Nodes = new List<Node>();
        }
        public string Kind { get; set; }
        public string ApiVersion { get; set; }
        public List<Node> Nodes { get; set; }
        public Node GetNode() => Nodes.First();
    }

    public class Node
    {
        public Node()
        {
            ExtraPortMappings = new List<ExtraPortMapping>();
        }
        public string Role { get; set; }
        public string Image { get; set; }
        public List<string> KubeadmConfigPatches { get; set; }
        public List<ExtraPortMapping> ExtraPortMappings { get; set; }
    }

    public class KubeadmConfigPatch
    {
        public string Kind { get; set; }
        public string NodeRegistration { get; set; }
    }

    public class NodeRegistration
    {
        public Dictionary<string, string> KubeletExtraArgs { get; set; }
    }

    public class ExtraPortMapping
    {
        public int ContainerPort { get; set; }
        public int HostPort { get; set; }
        public string Protocol => "TCP";
    }
}
