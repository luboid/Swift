using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift
{
    public class SwiftReaderException : ApplicationException
    {
        public SwiftReaderException(string message) : 
            base(message) { }
    }
}
