using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift
{
    public class InvalidCharcterException : SwiftReaderException
    {
        public InvalidCharcterException(int index, int line, int character)
            : base(string.Format("Invalid character in message #{0} at ({1}:{2}).", index, line, character))
        { }
    }
}
