using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift
{
    public class InvalidCharcterException : SwiftReaderException
    {
        public InvalidCharcterException(int line, int character)
            : base(string.Format("Invalid character at ({0}:{1}).", line, character))
        { }
    }
}
