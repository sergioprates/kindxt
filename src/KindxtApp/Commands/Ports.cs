using KindxtApp.Commands.Kind;

namespace KindxtApp.Commands
{
    public static class Ports
    {
        public static ExtraPortMapping SqlServer => new ExtraPortMapping
        {
            ContainerPort = 30000,
            HostPort = 1433
        };
        public static ExtraPortMapping Postgres => new ExtraPortMapping
        {
            ContainerPort = 30001,
            HostPort = 5432
        };
        public static ExtraPortMapping Adminer => new ExtraPortMapping
        {
            ContainerPort = 30002,
            HostPort = 9000
        };
    }
}
