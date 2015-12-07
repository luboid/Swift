using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift
{
    public class InvalidBlockException : SwiftReaderException
    {
        InvalidBlock[] _invalidBlocks;

        public InvalidBlockException(params InvalidBlock[] invalidBlocks)
            : base("Invalid blocks.")
        {
            _invalidBlocks = invalidBlocks;
        }

        public InvalidBlock[] InvalidBlocks
        {
            get
            {
                return _invalidBlocks;
            }
        }

        public override string Message
        {
            get
            {
                var buffer = new StringBuilder();
                foreach (var b in _invalidBlocks)
                    foreach (var m in b.Messages)
                        buffer.Append(m);

                return buffer.ToString();
            }
        }
    }
}
