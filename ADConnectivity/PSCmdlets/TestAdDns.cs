using Heijden.DNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Text;
using Dusty.ADConnectivity;
using Dusty.Net;


namespace Dusty.ADConnectivity
{
    [Cmdlet(VerbsDiagnostic.Test, "AdDns")]
    [OutputType(typeof(bool))]
    public class TestAdDns : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        public AdDnsResolver[] DnsServer { get; set; }

        [Parameter(Mandatory = false, Position = 1)]
        public string AdSite { get; set; }
        
        private List<AdDnsResolver> resolvers;
        private List<AdDnsResponse> responses;
        
        protected override void BeginProcessing()
        {
            resolvers = new List<AdDnsResolver>();
        }

        protected override void ProcessRecord()
        {
            resolvers.AddRange(DnsServer);
        }

        protected override void EndProcessing()
        {
            if (MyInvocation.BoundParameters.ContainsKey(nameof(AdSite)))
            {
                resolvers.ForEach(r => r.AdSite = AdSite);
            }


            WriteObject(resolvers);
            

        }
    }
}
