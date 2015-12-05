using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift
{
    public class Field : Tag
    {
        string _fieldId;
        string _letter;

        public Field(string fieldId, string letter, int index) : base(fieldId + letter, index)
        {
            _fieldId = fieldId;
            _letter = letter ?? string.Empty;
        }

        public string FieldId { get { return _fieldId; } }
        public string Letter { get { return _letter; } }
    }
}
