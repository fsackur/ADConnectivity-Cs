using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Heijden.DNS;
using Dusty.Net;

namespace Dusty.ADConnectivity
{
    public static class DnsExtensionMethods
    {
        //Add a method to Heijden.DNS.Response to provide the DNS response as a string[]
        //it's up to the calling code to know how to parse the string (ip or hostname)
        public static string[] GetAnswerStrings(this Response response)
        {
            List<string> strings = new List<string>();

            foreach (var rr in response.Answers)
            {
                string s = rr.RECORD.ToString();
                if (rr.Type == Heijden.DNS.Type.SRV)
                {
                    s = System.Text.RegularExpressions.Regex.Replace(s, "\\d* \\d* \\d* ", "");
                }
                strings.Add(s);
            }

            return strings.ToArray<string>();
        }

        /*
        public static bool AnswerEquals(this Response response, Response comparison)
        {
            if (comparison == null || response == null)
            {
                return false;
            }
            var responseStrings = response.GetAnswerStrings();
            var comparisonStrings = comparison.GetAnswerStrings();
            Array.Sort<string>(responseStrings);
            Array.Sort<string>(comparisonStrings);

            return responseStrings.SequenceEqual<string>(comparisonStrings);
        }
        */
    }
}
