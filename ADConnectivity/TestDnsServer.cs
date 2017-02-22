using Heijden.DNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Text;

namespace ADConnectivity
{
    [Cmdlet(VerbsDiagnostic.Test, "DnsServer")]
    public class TestDnsServer : PSCmdlet
    {
        public Resolver resolver { get; private set; }


        [Parameter(Mandatory = true, Position = 0)]
        public IPAddress IpAddress { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        public string Domain { get; set; }

        protected override void BeginProcessing()
        {
            resolver = new Resolver(IpAddress, 53);
        }

        protected override void ProcessRecord()
        {
            WriteObject(IpAddress);
            WriteObject(resolver.GetHostByName(Domain));
        }


    }
}
