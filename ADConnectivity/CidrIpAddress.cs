using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;

namespace Dusty.Net
{
    public class CidrIpAddress : IPAddress
    {
        //Constructors from base class
        public CidrIpAddress(byte[] address) : base(address) { }
        public CidrIpAddress(long newAddress) : base(newAddress) { }
        public CidrIpAddress(byte[] address, long scopeid) : base(address, scopeid) { }

        //Extended constructors
        public CidrIpAddress(byte[] address, byte[] subnetMask) : base(address)
        {
            this.subnetMask = new SubnetMask(subnetMask);
        }

        public CidrIpAddress(long newAddress, long subnetMask) : base(newAddress)
        {
            this.subnetMask = new SubnetMask(subnetMask);
        }

        public CidrIpAddress(byte[] address, byte[] subnetMask, long scopeid) : base(address, scopeid)
        {
            this.subnetMask = new SubnetMask(subnetMask);
        }

        public CidrIpAddress(IPAddress ipaddress, SubnetMask subnetMask) : base(ipaddress.GetAddressBytes())
        {
            this.subnetMask = subnetMask;
        }

        private SubnetMask mask;
        public SubnetMask subnetMask {
            get
            {
                return mask;
            }
            set
            {
                if (value.AddressFamily != this.AddressFamily)
                {
                    throw new ArgumentException("Subnet mask is not of same family as IP address");
                }
                this.mask = value;
            }
        }

        public string ToCidrString()
        {
            return string.Format(
                "{0}/{1}",
                Regex.Replace(ToString(), "%.*$", ""),
                subnetMask.NetworkPrefixLength
            );
        }

        public bool IsInSameSubnet(IPAddress comparisonIp)
        {
            return Utils.IsInSameSubnet(this, comparisonIp);
        }
    }


}
