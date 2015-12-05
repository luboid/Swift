using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift
{
    public class InvalidBlock : IBaseBlock
    {
        string _blockId;
        Section[] _raw;
        List<string> _messages;

        public InvalidBlock(string blockId, Section[] section)
        {
            _raw = section;
            _blockId = blockId;
        }

        public Section[] Raw
        {
            get
            {
                return _raw;
            }
        }

        public string BlockId
        {
            get
            {
                return _blockId;
            }
        }

        public bool HasMessages
        {
            get
            {
                return _messages != null && _messages.Count != 0;
            }
        }

        public List<string> Messages
        {
            get
            {
                return _messages;
            }
        }

        public InvalidBlock AddMessage(string message)
        {
            if (_messages == null)
                _messages = new List<string>();
            _messages.Add(message);
            return this;
        }
    }
}