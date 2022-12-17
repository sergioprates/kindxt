namespace Kindxt.Kind
{
    public static class Ports
    {
        private static string _protocol = "TCP";

        public static ExtraPortMapping[] SqlServer => new ExtraPortMapping[]
        {
            new()
            {
                ContainerPort = 30000,
                HostPort = 1433,
                Protocol = _protocol
            }
        };
        public static ExtraPortMapping[] Postgres => new ExtraPortMapping[]
        {
            new()
            {
                ContainerPort = 30001,
                HostPort = 5432,
                Protocol = _protocol
            }
        };
        public static ExtraPortMapping[] PgAdmin => new ExtraPortMapping[]
        {
            new ()
            {
                ContainerPort = 30002,
                HostPort = 9000,
                Protocol = _protocol
            }
        };
        public static ExtraPortMapping[] NginxIngress => new ExtraPortMapping[]
        {
            new()
            {
                ContainerPort = 30003,
                HostPort = 8080,
                Protocol = _protocol
            }
        };
        public static ExtraPortMapping[] Istio => new ExtraPortMapping[]
        {
            new()
            {
                ContainerPort = 30004,
                HostPort = 8081,
                Protocol = _protocol
            },
            new()
            {
                ContainerPort = 30005,
                HostPort = 8082,
                Protocol = _protocol
            },
            new()
            {
                ContainerPort = 30006,
                HostPort = 8083,
                Protocol = _protocol
            },
        };
    }
}
