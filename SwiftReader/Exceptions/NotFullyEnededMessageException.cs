using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift
{
    public class NotFullyEnededMessageException : SwiftReaderException
    {
        public NotFullyEnededMessageException()
            : base("Not fully eneded message.")
        { }
    }
}
