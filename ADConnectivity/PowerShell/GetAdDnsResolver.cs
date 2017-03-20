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
    
    [Cmdlet(VerbsCommon.Get, "AdDnsResolver")]
    [OutputType(typeof(AdDnsResolver))]
    public class GetAdDnsResolver : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        [ValidateDnsHostnameOrIpAddress()]
        public string[] ComputerName { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        [ValidateDnsHostname()]
        public string AdDomain { get; set; }

        protected override void ProcessRecord()
        {
            
            List<IPAddress> ipList = new List<IPAddress>();

            foreach (string name in ComputerName)
            {
                if (Uri.CheckHostName(name.ToString()) == UriHostNameType.IPv4 ||
                        Uri.CheckHostName(name.ToString()) == UriHostNameType.IPv6)
                {
                    IPAddress ip = IPAddress.Parse(name);
                    WriteObject(new AdDnsResolver(new IPEndPoint(ip, 53), AdDomain));
                    continue;
                }

                try
                {
                    IPHostEntry host = Dns.GetHostEntry(name);
                    foreach (IPAddress ip in host.AddressList)
                    {
                        WriteObject(new AdDnsResolver(new IPEndPoint(ip, 53), AdDomain));
                    }
                }
                catch
                {
                    WriteWarning(string.Format("Unable to resolve hostname {0}", name));
                }

            } //end foreach name in ComputerName
        }  //end ProcessRecord

    }
}
