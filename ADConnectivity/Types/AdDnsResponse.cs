using Dusty.Net;
using Heijden.DNS;
using Dusty.Net;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Newtonsoft.Json;
using System;

namespace Dusty.ADConnectivity
{
    public class AdDnsResponse : IEqualityComparer<AdDnsResponse>
    {
        public AdDnsResponse(
            string dnsServer,
            string adDomain,
            Dictionary<string, DnsResponse> namedResponses
            )
        {
            this.AdDomain = adDomain;
            this.DnsServer = dnsServer;
            this.namedResponses = namedResponses;
        }

        public AdDnsResponse(
            string dnsServer,
            string adDomain,
            string adSite,
            Dictionary<string, DnsResponse> namedResponses
            ) : this (dnsServer, adDomain, namedResponses)
        {
            this.AdSite = adSite;
        }

        public string AdDomain { get; private set; }
        public string DnsServer { get; private set; }
        public string AdSite { get; private set; }
        private Dictionary<string, DnsResponse> namedResponses;

        public DnsResponse Pdc {
            get
            {
                DnsResponse retval;
                namedResponses.TryGetValue("PDC", out retval);
                return retval;
            }
        }
        
        public DnsResponse DomainARecords {
            get
            {
                DnsResponse retval;
                namedResponses.TryGetValue("DomainARecords", out retval);
                return retval;
            }
        }

        public string[] GetErrors()
        {
            return namedResponses
                .Values
                .Select(response => response.Error)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Distinct()
                .ToArray();
        }

        public DnsResponse GetResponse(string name)
        {
            return namedResponses[name];
        }

        public Dictionary<string, DnsResponse> GetResponses()
        {
            return namedResponses;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(namedResponses);
        }


        //an overload that lets us put each discrepancy in an out variable
        public bool Equals(AdDnsResponse comparison, out List<string> differences)
        {
            differences = new List<string>(1);
            if (comparison == null)
            {
                differences.Add("REF");
                return false;
            }
            if (this.AdDomain != comparison.AdDomain)
            {
                differences.Add("AdDomain");
                return false;
            }
            differences = (
                            from kvpThis in this.namedResponses
                            join kvpComp in comparison.namedResponses
                            on kvpThis.Key equals kvpComp.Key
                            where !kvpThis.Value.Equals(kvpComp.Value.Answers)
                            select kvpThis.Key
                        ).ToList<string>();

            return (differences.Count == 0);
        }
        


        public bool Equals(AdDnsResponse comparison)
        {
            return Equals(comparison, null);
        }

        

        // IEqualityComparer methods
        public bool Equals(AdDnsResponse x, AdDnsResponse y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(AdDnsResponse obj)
        {
            return ToString().GetHashCode();
        }
    }
}