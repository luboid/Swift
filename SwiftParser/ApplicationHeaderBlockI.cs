using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Swift
{
    public class ApplicationHeaderBlockI : ApplicationHeaderBlock
    {
        public static readonly Regex Pattern = new Regex("(I)([0-9]{3})([0-9A-Z]{12})([SNU]{1})([1-3]{1})([0-9]{3})", RegexOptions.Compiled);

        public ApplicationHeaderBlockI()
        {
            Type = 'I';
        }

        public string ReceiverAddress { get; set; }
        public char DeliveryMonitoring { get; set; }
        public string ObsolescencePeriod { get; set; }
    }
}
