using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift
{
    public class TooManySectionsException : SwiftReaderException
    {
        public TooManySectionsException(int index)
            : base(string.Format("Too many sections at hight level in message #{0}.", index))
        { }
    }
}
