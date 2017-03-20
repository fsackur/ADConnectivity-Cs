using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Heijden.DNS;
using Dusty.Net;

namespace Dusty.ADConnectivity
{
    public class DnsResolver
    {
        public DnsResolver(IPEndPoint DnsServer)
        {
            this.resolver = new Resolver(DnsServer);
        }

        public DnsResolver(IPAddress DnsServer)
        {
            this.resolver = new Resolver(new IPEndPoint(DnsServer, 53));
        }

        private Resolver resolver;
        public string DnsServer { get { return resolver.DnsServer; } }

        public DnsResponse Query(string name, QType qtype)
        {
            Response r = resolver.Query(name, qtype);

            return new DnsResponse(
                r.GetAnswerStrings(),
                r.Error
                );

        }
    }
}
