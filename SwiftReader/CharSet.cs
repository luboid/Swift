using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift
{
    public class CharSet
    {
        public class ContiniusCharSet
        {
            public char Begin { get; set; }
            public char End { get; set; }
        }

        public ContiniusCharSet[] ContiniusCharSets { get; set; }
        public char[] OtherSymbols { get; set; }
    }
}
