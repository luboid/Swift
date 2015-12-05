using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift
{
    public class UserHeaderBlock : BaseBlockCollection<Tag>
    {
        public static readonly string BLOCK_ID = "3";

        public UserHeaderBlock()
        { }

        protected override Tag New(string id)
        {
            return new Tag(id, -1);
        }

        public override string BlockId
        {
            get
            {
                return BLOCK_ID;
            }
        }

        public static IBaseBlock[] Create(Section[] sections)
        {
            var b = new UserHeaderBlock();
            for (int i = 0, l = sections.Length; i < l; i++)
            {
                var headers = sections[i].Sections;
                for (int j = 0, jl = headers.Count; j < jl; j++)
                {
                    var header = headers[j];
                    // here can try to validate data and create specific with more detailed information
                    b.List.Add(new Tag(header.BlockId, j) { Value = header.Data });
                }
            }
            return new[] { b };
        }
    }
}
