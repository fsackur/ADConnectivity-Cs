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
    /*
     * We want a return type for the methods in AdDnsResolver that encapsulates
     * the underlying DnsResponse objects.
     * 
     * We want to match the extensibility of AdDnsResolver, so the actual DNS 
     * responses are backed by a dictionary which we can access with GetResponse 
     * and GetResponses methods
     * 
     * In the context of sanity-checking the DNS servers in an AD domain, it's
     * useful to know if all DNS servers return the same records, so we implement
     * Equals
     */
    public class AdDnsResponse : IEqualityComparer<AdDnsResponse>, IEquatable<AdDnsResponse>
    {
        public AdDnsResponse(
            Dictionary<string, DnsResponse> namedResponses,
            string dnsServer,
            string adDomain,
            string adSite = null
        )
        {
            this.namedResponses = namedResponses;
            this.AdDomain = adDomain;
            this.DnsServer = dnsServer;
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

        public List<string> GetErrors()
        {
            List<string> errors = new List<string>();
            foreach (var kvp in namedResponses)
            {
                if (kvp.Value == null)
                {
                    errors.Add($"{kvp.Key}: No record found");
                }
                else
                {
                    errors.AddRange(
                        from e in kvp.Value.Errors
                        select $"{kvp.Key}: {e}"
                        );
                }
            }
            return errors;
        }

        public DnsResponse GetResponse(string name)
        {
            DnsResponse retval;
            namedResponses.TryGetValue(name, out retval);
            return retval;
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
            if (comparison == null) { throw new ArgumentNullException(); }

            differences = new List<string>(1);
            /*
            if (this.AdDomain != comparison.AdDomain)
            {
                differences.Add("AdDomain");
                return false;
            }
            if (this.AdSite != comparison.AdSite)
            {
                differences.Add("AdSite");
                return false;
            }
            */
            differences = new List<string>(
                from kvpThis in this.namedResponses
                join kvpComp in comparison.namedResponses
                on kvpThis.Key equals kvpComp.Key
                where
                    kvpThis.Value == null ?
                    kvpComp.Value != null :
                    !kvpThis.Value.Equals(kvpComp.Value)
                select kvpThis.Key
                );
            
            return (differences.Count == 0);
        }
        
        public bool Equals(AdDnsResponse comparison)
        {
            List<string> outval;
            return Equals(comparison, out outval);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        // IEqualityComparer methods
        public bool Equals(AdDnsResponse x, AdDnsResponse y)
        {
            if (x == null && y == null) { return true; }
            if (x == null && y != null) { return false; }
            return x.Equals(y);
        }

        public int GetHashCode(AdDnsResponse obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}