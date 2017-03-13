using Dusty.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Heijden.DNS;

namespace Dusty.ADConnectivity
{
    public class AdDnsResolver : DnsResolver
    {
        private readonly string pdcMultipleRecordError = "Multiple records for PDC locator";

        public AdDnsResolver(IPEndPoint DnsServer, string AdDomain) : this(DnsServer)
        {
            this.AdDomain = AdDomain;
        }

        //hide the parent's constructor, because it makes no sense to have this calss without the AdDomain property
        private AdDnsResolver(IPEndPoint DnsServer) : base(DnsServer)
        {
        }

        public string AdDomain { get; private set; }

        public Response Pdc { get; private set; }

        public Response DomainARecords { get; private set; }

        public Response QueryPdc()
        {
            string question = $"_ldap._tcp.pdc._msdcs.[_adDomain]";
            Pdc = Query(question, QType.SRV);
            if (Pdc.Answers.Count > 0)
            {
                Pdc.Error = pdcMultipleRecordError;
            }
            return Pdc;
        }

        public Response QueryDomainARecords()
        {
            DomainARecords = Query(AdDomain, QType.A);
            return DomainARecords;
        }

        public AdDnsResolver QueryAd()
        {
            QueryPdc();
            QueryDomainARecords();
            return this;
        }

        public bool Equals(AdDnsResolver comparison)
        {
            if (comparison == null) { return false; }
            return (
                Pdc.AnswerEquals(comparison.Pdc) &&
                DomainARecords.AnswerEquals(comparison.DomainARecords)
            );
        }
    }
}
