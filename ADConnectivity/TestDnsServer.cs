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

    [Cmdlet(VerbsCommon.Get, "DnsServer", DefaultParameterSetName = "IpAddress")]
    public class GetDnsServer : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = "IpAddress")]
        public IPAddress[] IpAddress { get; set; }

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = "Hostname")]
        public string[] Hostname { get; set; }



        protected override void ProcessRecord()
        {
            if (ParameterSetName == "IpAddress")
            {
                foreach (IPAddress ip in IpAddress)
                {
                    WriteObject(new Resolver(new IPEndPoint(ip, 53)));
                    Console.WriteLine("Processed one");
                }
            }
            else
            {
                List<IPAddress> ipList = new List<IPAddress>();

                foreach (string name in Hostname)
                {
                    try
                    {
                        IPHostEntry host = System.Net.Dns.GetHostEntry(name);
                        foreach (IPAddress ip in host.AddressList)
                        {
                            WriteObject(new Resolver(new IPEndPoint(ip, 53)));
                            Console.WriteLine("Processed one");
                        }
                    }
                    catch
                    {
                        WriteWarning(string.Format("Unable to resolve hostname {0}", name));
                    }
                   
                } //end foreach name in Hostname
            }

        }  //end ProcessRecord
    }
}
