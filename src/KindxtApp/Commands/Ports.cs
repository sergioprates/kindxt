using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KindxtApp.Commands.Kind;

namespace KindxtApp.Commands
{
    public class Ports
    {
        public static ExtraPortMapping SqlServer => new ExtraPortMapping
        {
            ContainerPort = 30000,
            HostPort = 1433
        };
    }
}
