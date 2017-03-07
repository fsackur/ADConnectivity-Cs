using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Dusty.Net
{
    public class SubnetMask : IPAddress
    {
        //Constructors
        public SubnetMask(byte[] address) : base(address)
        {
            this.length = this.GetNetworkPrefixLength();
        }

        public SubnetMask(long newAddress) : base(newAddress)
        {
            this.length = this.GetNetworkPrefixLength();
        }

        public SubnetMask(byte[] address, long scopeid) : base(address, scopeid)
        {
            this.length = this.GetNetworkPrefixLength();
        }

        //Only interesting property over IPaddress is the concept of Network prefix length (e.g. '24' in '192.168.0.1/24')
        private int length;
        public int NetworkPrefixLength
        {
            get { return this.length; }
        }


        

    }
}
