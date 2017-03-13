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
    [Cmdlet(VerbsCommon.Get, "DnsResponse", DefaultParameterSetName = "SimpleResponse")]
    [OutputType("string", ParameterSetName = new string[] { "SimpleResponse" })]
    [OutputType(typeof(Response), ParameterSetName = new string[] { "StructuredResponse" })]
    public class GetDnsResponse : PSCmdlet
    {
        [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
        public DnsResolver DnsResolver { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        public string Domain { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = "SimpleResponse")]
        public SwitchParameter SimpleResponse { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "StructuredResponse")]
        public SwitchParameter StructuredResponse { get; set; }

        [Parameter(Mandatory = true)]
        public QType QueryType { get; set; }


        protected override void ProcessRecord()
        {

            Response r = DnsResolver.Query(Domain, QueryType);

            
            if (ParameterSetName != "SimpleResponse")
            {
                WriteObject(r);
            }
            else
            {
                List<string> strings = new List<string>();

                foreach (var rr in r.Answers)
                {
                    string s = rr.RECORD.ToString();
                    if (rr.Type == Heijden.DNS.Type.SRV)
                    {
                        s = System.Text.RegularExpressions.Regex.Replace(s, "\\d* \\d* \\d* ", "");
                    }
                    strings.Add(s);
                }

                foreach (var s in strings) { WriteObject(s); }
            }
            
        }


    }

}
