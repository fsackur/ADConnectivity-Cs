//https://www.codeproject.com/Articles/23673/DNS-NET-Resolver-C
//Install-Package Heijden.DNS
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
    [Cmdlet(VerbsDiagnostic.Test, "DnsResolver")]
    [OutputType("AdDnsResponse")]
    public class TestAdDns : PSCmdlet
    {
        [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
        public DnsResolver DnsResolver { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        public string Domain { get; set; }
        


        protected override void ProcessRecord()
        {
            WriteObject("");

        }


    }
    
}
