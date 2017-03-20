using System;
using System.Linq;

namespace Dusty.ADConnectivity
{
    public class DnsResponse
    {
        public string Error { get; private set; }
        public string[] Answers { get; private set; }

        public DnsResponse(string[] answers, string error)
        {
            Array.Sort(answers);
            this.Answers = answers;
            this.Error = error;
        }

        public override string ToString()
        {
            return String.Join(" ", this.Answers);
        }

        public bool Equals(DnsResponse comparison)
        {
            return Answers.SequenceEqual(comparison.Answers);
        }
    }
}