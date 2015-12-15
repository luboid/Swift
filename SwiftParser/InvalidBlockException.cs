using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift
{
    public class InvalidBlockException : SwiftReaderException
    {
        Section _raw;
        InvalidBlock[] _invalidBlocks;

        public InvalidBlockException(Section message, params InvalidBlock[] invalidBlocks)
            : base("Invalid blocks.")
        {
            _raw = message;
            _invalidBlocks = invalidBlocks;
        }

        public Section Raw
        {
            get
            {
                return _raw;
            }
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
                buffer.AppendFormat("Message #{0}, ", _raw.Index);
                foreach (var b in _invalidBlocks)
                {
                    buffer.AppendFormat("Block ({0}): ", b.BlockId);
                    foreach (var m in b.Messages)
                        buffer.Append(m);
                }
                return buffer.ToString();
            }
        }
    }
}
