using System;
using System.Collections.Generic;
using System.Linq;

namespace Dusty.ADConnectivity
{
    //in C# we would probably use the pattern of a string[] return type with an out variable for errors
    //PowerShell doesn't tend to use that pattern. It would work, but would be foreign to most users
    //so we want an return type for DNS queries that has an 'Error' field
    //Tuples are not handled well in PowerShell
    //hence, this class
    public class DnsResponse : IEqualityComparer<DnsResponse>
    {
        public DnsResponse(string[] answers, string error)
        {
            Array.Sort(answers);
            this.Answers = answers;
            this.Error = error;
        }

        public string Error { get; private set; }
        public string[] Answers { get; private set; }

        public override string ToString()
        {
            return String.Join(",", this.Answers);
        }

        public bool Equals(DnsResponse comparison)
        {
            return Answers.SequenceEqual(comparison.Answers);
        }

        public bool Equals(DnsResponse x, DnsResponse y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(DnsResponse obj)
        {
            //we want instances with identical answers to return an identical hashcode
            return String.Join("", this.Answers).GetHashCode();
        }
    }
}