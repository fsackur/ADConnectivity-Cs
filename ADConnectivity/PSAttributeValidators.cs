using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace Dusty.Net
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ValidateDnsHostnameAttribute : ValidateEnumeratedArgumentsAttribute
    {
        private string Name { get; } = nameof(ValidateDnsHostnameAttribute).Replace("Attribute", "");

        protected override void ValidateElement(object element)
        {
            if (Uri.CheckHostName(element.ToString()) != UriHostNameType.Dns)
            {
                throw new ValidationMetadataException($"'{element}' is not a valid DNS hostname");
            }
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ValidateDnsHostnameOrIpAddressAttribute : ValidateEnumeratedArgumentsAttribute
    {
        private string Name { get; } = nameof(ValidateDnsHostnameAttribute).Replace("Attribute", "");

        protected override void ValidateElement(object element)
        {
            if (
                Uri.CheckHostName(element.ToString()) != UriHostNameType.Dns &&
                Uri.CheckHostName(element.ToString()) != UriHostNameType.IPv4 &&
                Uri.CheckHostName(element.ToString()) != UriHostNameType.IPv6
            )
            {
                throw new ValidationMetadataException($"'{element}' is not a valid DNS hostname or IP address");
            }
        }
    }
}
