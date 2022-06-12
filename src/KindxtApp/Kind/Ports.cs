namespace KindxtApp.Kind
{
    public static class Ports
    {
        public static ExtraPortMapping SqlServer => new()
        {
            ContainerPort = 30000,
            HostPort = 1433
        };
        public static ExtraPortMapping Postgres => new()
        {
            ContainerPort = 30001,
            HostPort = 5432
        };
        public static ExtraPortMapping PgAdmin => new()
        {
            ContainerPort = 30002,
            HostPort = 9000
        };
        public static ExtraPortMapping NginxIngress => new()
        {
            ContainerPort = 30003,
            HostPort = 8080
        };
    }
}
