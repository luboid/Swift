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
        public static readonly Regex Pattern = new Regex("(I)(?<MessageType>[0-9]{3})(?<ReceiversAddress>[0-9A-Z]{12})(?<Priority>[SNU]{1})(?<DeliveryMonitoring>[1-3]{1})(?<ObsolescencePeriod>[0-9]{3})", RegexOptions.Compiled);

        public ApplicationHeaderBlockI()
        {
            Type = 'I';
        }

        public string ReceiversAddress { get; set; }
        public char DeliveryMonitoring { get; set; }
        public string ObsolescencePeriod { get; set; }
    }
}
