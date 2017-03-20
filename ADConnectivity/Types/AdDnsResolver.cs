using Dusty.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Heijden.DNS;
using Dusty.Net;
using System.Management;

namespace Dusty.ADConnectivity
{
    public class AdDnsResolver : DnsResolver
    {
        private readonly string pdcMultipleRecordError = "Multiple records for PDC locator";

        public AdDnsResolver(IPEndPoint DnsServer, string AdDomain) : base(DnsServer)
        {
            this.AdDomain = AdDomain;
        }

        public AdDnsResolver(IPAddress DnsServer, string AdDomain) : base(DnsServer)
        {
            this.AdDomain = AdDomain;
        }

        public AdDnsResolver(IPAddress DnsServer) : base(DnsServer)
        {
            string ComputerName = Environment.GetEnvironmentVariable("COMPUTERNAME");
            ManagementObject cs = new ManagementObject(
                $"Root\\CIMv2:Win32_ComputerSystem.Name='{ComputerName}'"
                );
            this.AdDomain = cs.GetPropertyValue("Domain").ToString();
        }

        public AdDnsResolver(string DnsServer) : this(IPUtils.ParseAndResolve(DnsServer)[0]) { }


        //hide the parent's constructor, because it makes no sense to have this class without the AdDomain property
        private AdDnsResolver(IPEndPoint DnsServer) : base(DnsServer)
        {
        }

        public string AdDomain { get; private set; }

        public string AdSite { get; set; }

        public DnsResponse Pdc { get; private set; }

        public DnsResponse DomainARecords { get; private set; }

        public DnsResponse QueryPdc()
        {
            string question = string.Format("_ldap._tcp.pdc._msdcs.{AdDomain}", AdDomain);

            Pdc = base.Query($"_ldap._tcp.pdc._msdcs.{AdDomain}", QType.SRV);
            if (Pdc.Answers.Count() > 1)
            {
                Pdc = new DnsResponse(
                    null,
                    pdcMultipleRecordError
                    );
            }
            
            return Pdc;
        }

        public DnsResponse QueryDomainARecords()
        {
            DomainARecords = Query(AdDomain, QType.A);
            return DomainARecords;
        }

        public AdDnsResponse QueryAd()
        {
            return QueryAd(AdDnsResolver.DefaultAdQueries);
        }

        public AdDnsResponse QueryAd(Dictionary<string, Func<AdDnsResolver, DnsResponse>> adQueries)
        {
            return new AdDnsResponse(
                dnsServer: this.DnsServer,
                adDomain: this.AdDomain,
                adSite: this.AdSite,
                namedResponses: RunAdQueries(adQueries)
                );
        }

        private Dictionary<string, DnsResponse> RunAdQueries(Dictionary<string, Func<AdDnsResolver, DnsResponse>> adQueries)
        {
            var namedResponses = from name in adQueries.Keys
                                 let response = adQueries[name](this)
                                 select new KeyValuePair<string, DnsResponse>(name, response);
            return namedResponses.ToDictionary(x => x.Key, x => x.Value);

        }

        public static Dictionary<string, Func<AdDnsResolver, DnsResponse>> DefaultAdQueries =
                    new Dictionary<string, Func<AdDnsResolver, DnsResponse>>(10)
        {
            { "PDC", resolver => resolver.QueryPdc() },
            { "DomainARecords", resolver => resolver.QueryDomainARecords() }

        };

    }
}
