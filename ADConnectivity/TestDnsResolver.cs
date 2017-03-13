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

namespace Dusty.Net
{
    [Cmdlet(VerbsDiagnostic.Test, "DnsResolver", DefaultParameterSetName = "A")]
    [OutputType("bool", ParameterSetName = new string[] { "ActiveDirectory" })]
    [OutputType("string", ParameterSetName = new string[] { "SimpleResponse" })]
    [OutputType("string", ParameterSetName = new string[] { "SimpleResponse" })]
    public class TestDnsResolver : PSCmdlet
    {
        [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
        public DnsResolver DnsResolver { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        public string Domain { get; set; }

        [Parameter(ParameterSetName = "A")]
        public SwitchParameter A { get; set; }

        [Parameter(ParameterSetName = "SRV")]
        public SwitchParameter SRV { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "QueryType")]
        public QType QueryType { get; set; }

        [Parameter(ParameterSetName = "ActiveDirectory")]
        public SwitchParameter TestActiveDirectory { get; set; }


        protected override void ProcessRecord()
        {
            if (ParameterSetName != "A" && ParameterSetName != "SRV")
            {
                QueryType = (QType)Enum.Parse(typeof(QType), ParameterSetName);
            }

            if (ParameterSetName != "ActiveDirectory")
            {
                var response = DnsResolver.Query(Domain, QueryType);

                WriteObject(response.GetAnswerStrings());

                return;
            } // end if != ActiveDirectory



        }


    }
    
}
