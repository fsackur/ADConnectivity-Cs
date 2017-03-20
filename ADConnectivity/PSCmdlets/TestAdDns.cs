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
    [Cmdlet(VerbsDiagnostic.Test, "AdDns")]
    [OutputType(typeof(bool))]
    class TestAdDns
    {

    }
}
