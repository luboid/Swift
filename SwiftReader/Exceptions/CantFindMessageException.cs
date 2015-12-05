using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift
{
    public class CantFindMessageException : SwiftReaderException
    {
        public CantFindMessageException(int pos)
            : base(string.Format("{0} symbols so far and can't find start of SWIFT message.", pos))
        { }
    }
}
