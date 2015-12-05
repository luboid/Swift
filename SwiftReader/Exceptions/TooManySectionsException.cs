using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift
{
    public class TooManySectionsException : SwiftReaderException
    {
        public TooManySectionsException()
            : base("Too many sections at hight level.")
        { }
    }
}
