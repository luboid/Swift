using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Swift
{
    public class TextBlockData : BaseBlockCollection<Field>
    {
        public TextBlockData()
        { }

        protected override Field New(string id)
        {
            return new Field(
                id.Substring(0, 2), 
                id.Length == 3 ? id.Substring(2, 1) : string.Empty);
        }

        public override char BlockType
        {
            get
            {
                return '4';
            }
        }
    }
}
