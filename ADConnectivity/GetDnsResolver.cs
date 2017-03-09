//https://www.codeproject.com/Articles/23673/DNS-NET-Resolver-C
//Install-Package Heijden.DNS
using Heijden.DNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Text;


namespace Dusty.Net
{
    
    [Cmdlet(VerbsCommon.Get, "DnsResolver")]
    [OutputType(typeof(DnsResolver))]
    public class GetDnsResolver : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        [ValidateDnsHostnameOrIpAddress()]
        public string[] ComputerName { get; set; }

        protected override void ProcessRecord()
        {
            
            List<IPAddress> ipList = new List<IPAddress>();

            foreach (string name in ComputerName)
            {
                if (System.Uri.CheckHostName(name.ToString()) == System.UriHostNameType.IPv4 ||
                        System.Uri.CheckHostName(name.ToString()) == System.UriHostNameType.IPv6)
                {
                    IPAddress ip = IPAddress.Parse(name);
                    WriteObject(new DnsResolver(new IPEndPoint(ip, 53)));
                    continue;
                }

                try
                {
                    IPHostEntry host = System.Net.Dns.GetHostEntry(name);
                    foreach (IPAddress ip in host.AddressList)
                    {
                        WriteObject(new DnsResolver(new IPEndPoint(ip, 53)));
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
