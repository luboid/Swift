using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift
{
    public class InvalidNewLineException : SwiftReaderException
    {
        public InvalidNewLineException(int line, int character)
            : base(string.Format("Invalid new line character sequences at ({0}:{1}).", line, character))
        { }
    }
}
