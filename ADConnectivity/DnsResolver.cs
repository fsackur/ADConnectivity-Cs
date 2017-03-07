using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Heijden.DNS;

namespace Dusty.Net
{
    public class DnsResolver : Resolver
    {
        public DnsResolver(IPEndPoint DnsServer) : base(DnsServer)
        {
        }
    }
}
