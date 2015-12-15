using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift
{
    public class InvalidNewLineException : SwiftReaderException
    {
        public InvalidNewLineException(int index, int line, int character)
            : base(string.Format("Invalid new line character sequences in message #{0} at ({1}:{2}).", index, line, character))
        { }
    }
}
