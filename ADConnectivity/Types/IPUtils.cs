using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Heijden.DNS;
using Dusty.Net;

namespace Dusty.Net
{
    public class IPUtils
    {
        public static IPAddress[] ParseAndResolve(string[] endpoints)
        {
            List<IPAddress> ips = new List<IPAddress>(endpoints.Length);

            foreach (var endpoint in endpoints)
            {
                if (Uri.CheckHostName(endpoint) == UriHostNameType.IPv4 ||
                    Uri.CheckHostName(endpoint) == UriHostNameType.IPv6)
                {
                    ips.Add(IPAddress.Parse(endpoint));
                    continue;
                }

                if (Uri.CheckHostName(endpoint) == UriHostNameType.Dns)
                {
                    ips.AddRange(Dns.GetHostEntry(endpoint).AddressList);
                    continue;
                }

                throw new ArgumentException($"'{endpoint}' is not a valid DNS hostname or IP address");

            }

            return ips.ToArray();
        }

        public static IPAddress[] ParseAndResolve(string endpoint)
        {
            return ParseAndResolve(new string[] { endpoint });
        }
    }
}
