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
    [Cmdlet(VerbsDiagnostic.Test, "DnsResolver", DefaultParameterSetName = "A")]
    [OutputType("bool", ParameterSetName = new string[] { "ActiveDirectory" })]
    public class TestDnsResolver : PSCmdlet
    {
        [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
        public Resolver DnsResolver { get; set; }

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
                switch (QueryType)
                {
                    case QType.A:
                        WriteObject(response.RecordsA);
                        break;
                    case QType.SRV:
                        WriteObject(response.RecordsSRV);
                        break;
                    case QType.CNAME:
                        WriteObject(response.RecordsCNAME);
                        break;
                    case QType.PTR:
                        WriteObject(response.RecordsPTR);
                        break;
                    case QType.NS:
                        WriteObject(response.RecordsNS);
                        break;
                    case QType.SOA:
                        WriteObject(response.RecordsSOA);
                        break;
                    case QType.TXT:
                        WriteObject(response.RecordsTXT);
                        break;
                    default:
                        //You can query the Resolver object's cache
                        break;
                }

                return;
            } // end if != ActiveDirectory



        }


    }
    
}
