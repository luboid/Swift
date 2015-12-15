using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift
{
    public class TooDeepException : SwiftReaderException
    {
        public TooDeepException(int index, int deep)
            : base(string.Format("Nesting of the sections is greater than the permissible ({0}) in message #{1}.", deep, index))
        { }
    }
}
