using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Dusty.Net
{
    public static class Utils
    {
        public static bool IsInSameSubnet(CidrIpAddress reference, IPAddress comparison)
        {
            if (reference.AddressFamily != comparison.AddressFamily)
            {
                throw new ArgumentException("reference and comparison IP addresses are not of same address family");
            }

            byte[] refBytes = reference.GetAddressBytes();
            byte[] compBytes = comparison.GetAddressBytes();

            //TODO
            return true;

        }

        public static bool IsInSameSubnet(IPAddress reference, IPAddress comparison, SubnetMask subnetMask)
        {
            return IsInSameSubnet(new CidrIpAddress(reference, subnetMask), comparison);
        }

        
    }
}
