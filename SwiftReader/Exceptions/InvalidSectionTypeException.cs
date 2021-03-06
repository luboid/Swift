﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift
{
    public class InvalidSectionTypeException : SwiftReaderException
    {
        public InvalidSectionTypeException(int index, string key, int startPos, int endPos)
            : base(string.Format("Invalid section type ({0}) at ({1}:{2}) in message #{3}.", key, startPos, endPos, index))
        { }
    }
}
