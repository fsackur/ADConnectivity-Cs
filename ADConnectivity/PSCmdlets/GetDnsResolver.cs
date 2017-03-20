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
using System.Management;

namespace Dusty.ADConnectivity
{

    [Cmdlet(VerbsCommon.Get, "DnsResolver", DefaultParameterSetName = "DnsOnly")]
    [OutputType(typeof(DnsResolver), ParameterSetName = (new string[1] { "DnsOnly" }))]
    [OutputType(typeof(AdDnsResolver), ParameterSetName = (new string[2] { "AdSpecifiedDomain", "AdMachineDomain" }))]
    public class GetDnsResolver : PSCmdlet
    {
        //for ease of use, we want to accept hostnames or ipaddresses
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        [ValidateDnsHostnameOrIpAddress()]
        public string[] DnsServer { get; set; }

        [Parameter(Mandatory = true, Position = 1, ParameterSetName = "AdSpecifiedDomain")]
        [ValidateDnsHostname()]
        public string AdDomain { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "AdMachineDomain")]
        public SwitchParameter UseMachineDomain { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "AdMachineDomain")]
        public SwitchParameter UseMachineDnsServers { get; set; }

        protected override void BeginProcessing()
        {
            if (MyInvocation.BoundParameters.ContainsKey(nameof(UseMachineDomain)))
            {
                string ComputerName = Environment.GetEnvironmentVariable("COMPUTERNAME");
                ManagementObject cs = new ManagementObject(
                    $"Root\\CIMv2:Win32_ComputerSystem.Name='{ComputerName}'"
                    );
                AdDomain = cs.GetPropertyValue("Domain").ToString();
            }
        }

        protected override void ProcessRecord()
        {
            
            foreach (string name in DnsServer)
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

            } //end foreach name in DnsServer
        }  //end ProcessRecord

    }
}
