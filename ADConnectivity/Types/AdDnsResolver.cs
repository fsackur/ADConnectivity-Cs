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
    /* 
     * We want to represent a DNS server that resolves a given AD domain
     * 
     * The main method is Query, which can do any DNS query you want
     * 
     * In the context of AD, we usually want to further resolve SRV
     * records to IP addresses, so there is a QuerySRV method
     * 
     * The SRV record for the PDC and the A record(s) for the root of the 
     * domain are both particularly important, so we include
     * PDC and DomainARcords properties and QueryPdc and QueryDomainARecords
     * methods
     * 
     * We want a certain amount of extensibility, so the actual DNS responses
     * are backed by a dictionary
     * 
     * The extensibility is provided by a static dictionary. You can add
     * lambda expressions that take an instance of AdResolver and return 
     * a DnsResponse
     */
    public class AdDnsResolver : DnsResolver
    {
        private readonly string pdcMultipleRecordError = "Multiple records for PDC locator";

        public AdDnsResolver(IPEndPoint DnsServer, string AdDomain, string AdSite = null) : base(DnsServer)
        {
            this.AdDomain = AdDomain;
            this.AdSite = AdSite;
        }

        public AdDnsResolver(IPAddress DnsServer, string AdDomain) : base(DnsServer)
        {
            this.AdDomain = AdDomain;
            this.AdSite = AdSite;
        }

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
            string locator = $"_ldap._tcp.pdc._msdcs.{AdDomain}";
            Pdc = QuerySrv(locator);

            if (Pdc.Answers.Count() > 1)
            {
                string error = Pdc.Error == null ?
                    pdcMultipleRecordError :
                    $"{pdcMultipleRecordError};{Pdc.Error}";
                Pdc = new DnsResponse(Pdc.Answers, error);
            }

            return Pdc;
        }

        public DnsResponse QuerySrv(string locator)
        {
            List<string> ipAnswers = new List<string>();
            List<string> errors = new List<string>();

            var response = Query(locator, QType.SRV);
            if (response.Error != null) { errors.Add(response.Error); }

            foreach (var name in response.Answers)
            {
                var secondResponse = Query(name);
                if (secondResponse.Error != null) { errors.Add(secondResponse.Error); }
                ipAnswers.AddRange(secondResponse.Answers);
            }

            return new DnsResponse(
                ipAnswers.ToArray<string>(), 
                String.Join(";", errors.ToArray())
                );
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
            //Key = name of test (PDC, SiteLDAP etc)
            //Value = response for that test
            var namedResponses =
                from name in adQueries.Keys
                let response = adQueries[name](this)
                select new KeyValuePair<string, DnsResponse>(name, response);

            return new AdDnsResponse(
                dnsServer: this.DnsServer,
                adDomain: this.AdDomain,
                adSite: this.AdSite,
                namedResponses: namedResponses.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                );
        }


        public static Dictionary<string, Func<AdDnsResolver, DnsResponse>> DefaultAdQueries =
                    new Dictionary<string, Func<AdDnsResolver, DnsResponse>>(10)
        {
            { "PDC", resolver => resolver.QueryPdc() },
            { "DomainARecords", resolver => resolver.QueryDomainARecords() },
            { "SiteLDAP", resolver =>
                resolver.AdSite == null ?
                null :
                resolver.QuerySrv($"_ldap.{resolver.AdSite}.{resolver.AdDomain}") }
        };

    }
}
