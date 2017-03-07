using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Dusty.Net
{
    public static class ExtensionMethods
    {
        public static BitArray GetAddressBits(this IPAddress ipaddress)
        {
            int length = GetBitLength(ipaddress);
            BitArray bitArray = new BitArray(length);

            byte[] bytes = ipaddress.GetAddressBytes();

            for (int i = 0; i < bytes.Length; i++)
            {
                int bitpos = 0x80;
                for (int j = 0; j < 8; j++)
                {
                    bitArray[i*8 + j] = (bytes[i] & bitpos) != 0;
                    bitpos = bitpos >> 1;
                }
            }

            return bitArray;
        }

        public static int GetBitLength(this IPAddress ipaddress)
        {
            return ipaddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6 ? 128 : 32;
        }


        public static string ToBinaryString(this IPAddress ipaddress)
        {
            int length = GetBitLength(ipaddress);
            BitArray bits = ipaddress.GetAddressBits();
            StringBuilder sb = new StringBuilder(length);
            foreach (var bit in bits)
            {
                sb.Append((bool)bit ? "1" : "0");
            }
            return sb.ToString();
        }



        public static int GetNetworkPrefixLength(this SubnetMask mask)
        {
            string errHostBits = "Subnet mask contains bits in host section";
            int length = 0;
            bool reachedEndOfPrefix = false;

            foreach (var _byte in mask.GetAddressBytes())
            {
                if (reachedEndOfPrefix)
                {
                    if (_byte == 0)
                    {
                        break;   //move on to next byte
                    }
                    else
                    {
                        throw new ArgumentException(errHostBits);
                    }

                }

                //bitwise operators return int
                int bits = _byte;

                //check each byte, 1 bit at a time
                for (int i = 0; i < 8; i++)
                {
                    //check most significant bit is 1
                    //0x80 = binary 10000000
                    if ((bits & 0x80) == 0x80)
                    {
                        //bit is 1
                        length++;
                        //shift right, i.e. move the next bit into the 8th position. We need to be careful to only ever examine the least-significant 8 bits
                        bits = bits << 1;
                    }
                    else
                    {
                        reachedEndOfPrefix = true;

                        //are there any bits left?
                        if ((bits & 0x7F) > 0)
                        {
                            throw new ArgumentException(errHostBits);
                        }
                        else
                        {
                            break;   //move on to next byte
                        }
                    }
                }

            }

            return length;
        }
    }
}
