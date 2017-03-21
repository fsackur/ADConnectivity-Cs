using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Heijden.DNS;
using Dusty.Net;
using System.Text.RegularExpressions;

namespace Dusty.ADConnectivity
{
    /*
     * This is named DnsResolver to disambiguate from the DnsServer management module
     * 
     * This is a wrapper for Heijden.DNS.Resolver, because we want simpler output
     */
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

        private Heijden.DNS.Resolver resolver;

        public string DnsServer
        {
            get
            {
                return resolver.DnsServer;
            }
        }

        public override string ToString()
        {
            return DnsServer;
        }

        public DnsResponse Query(string name, QType qtype = QType.A)
        {
            //this is a very raw object, very close to the underlying DNS protocol
            Heijden.DNS.Response response = resolver.Query(name, qtype);

            var answerStrings = new List<string>();
            var errors = new List<string>();
            if (!String.IsNullOrWhiteSpace(response.Error)) { errors.Add(response.Error); }

            foreach (var answer in response.Answers)
            {
                string answerString = answer.RECORD.ToString();
                if (answer.Type == Heijden.DNS.Type.SRV)
                {
                    //discard the priority/weight/port fields for SRV records
                    answerString = Regex.Replace(answerString, "\\d* \\d* \\d* ", "");
                }
                answerStrings.Add(answerString);
            }

            return new DnsResponse(answerStrings, errors);

        }
    }
}
