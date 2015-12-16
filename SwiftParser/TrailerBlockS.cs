using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift
{
    public class TrailerSBlock : BaseBlockCollection<Tag>
    {
        public static readonly string BLOCK_ID = "S";

        public TrailerSBlock()
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
            var b = new TrailerSBlock();
            for (int i = 0, l = sections.Length; i < l; i++)
            {
                var headers = sections[i].Sections;
                for (int j = 0, jl = headers.Count; j < jl; j++)
                {
                    var header = headers[j];
                    var t = new Tag(header.BlockId, j);
                    if (TextBlockParser.ParseHeader(t, header.Data))
                    {
                        b.List.Add(t);
                    }
                    else
                    {
                        return new[] { new InvalidBlock(BLOCK_ID, sections)
                            .AddMessage(string.Format("Invalid header block {0}.", header.BlockId)) };
                    }
                }
            }
            return new[] { b };
        }
    }
}
