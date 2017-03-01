//https://www.codeproject.com/Articles/23673/DNS-NET-Resolver-C
//Install-Package Heijden.DNS
using Heijden.DNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Text;

namespace ADConnectivity
{
    [Cmdlet(VerbsDiagnostic.Test, "DnsResolver")]
    public class TestDnsResolver : PSCmdlet
    {
        public Resolver resolver { get; private set; }


        [Parameter(Mandatory = true, Position = 0)]
        public Resolver DnsResolver { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        public string Domain { get; set; }

        protected override void ProcessRecord()
        {
            WriteObject(DnsResolver);
            WriteObject(resolver.GetHostByName(Domain));
        }


    }
    
}
