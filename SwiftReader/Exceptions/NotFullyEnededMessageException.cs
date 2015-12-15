using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift
{
    public class NotFullyEnededMessageException : SwiftReaderException
    {
        public NotFullyEnededMessageException(int index)
            : base(string.Format("Not fully eneded message #{0}.", index))
        { }
    }
}
