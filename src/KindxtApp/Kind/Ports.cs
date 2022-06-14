namespace Kindxt.Kind
{
    public static class Ports
    {
        private static string _protocol = "TCP";
        public static ExtraPortMapping SqlServer => new()
        {
            ContainerPort = 30000,
            HostPort = 1433,
            Protocol = _protocol
        };
        public static ExtraPortMapping Postgres => new()
        {
            ContainerPort = 30001,
            HostPort = 5432,
            Protocol = _protocol
        };
        public static ExtraPortMapping PgAdmin => new()
        {
            ContainerPort = 30002,
            HostPort = 9000,
            Protocol = _protocol
        };
        public static ExtraPortMapping NginxIngress => new()
        {
            ContainerPort = 30003,
            HostPort = 8080,
            Protocol = _protocol
        };
    }
}
