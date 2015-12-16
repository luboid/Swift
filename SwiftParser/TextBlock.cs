using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Swift
{
    public class TextBlock : BaseBlockCollection<Tag>
    {
        public static readonly string BLOCK_ID = "4";

        public TextBlock()
        { }

        protected override Tag New(string id)
        {
            //label.Match(id);
            //if new Field(Id,Letter)
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
            TextBlock b = new TextBlock();
            foreach (var section in sections)
            {
                if (section.NoData)// header section
                {
                    var list = section.Sections;
                    for (int i = 0, l = list.Count; i < l; i++)
                    {
                        var sub = list[i];
                        var t = new Tag(sub.BlockId, i);
                        if (TextBlockParser.ParseHeader(t, sub.Data))
                        {
                            b.List.Add(t);
                        }
                        else
                        {
                            return new[] { new InvalidBlock(BLOCK_ID, sections)
                            .AddMessage(string.Format("Invalid header block {0}.", sub.BlockId)) };
                        }
                    }
                }
                else
                {
                    if (TextBlockParser.IsDataBlock(section))
                    {
                        string errorMessage; IList<Tag> tags;
                        if (TextBlockParser.Parse(section.Data, out errorMessage, out tags))
                        {
                            foreach (var t in tags)
                                b.List.Add(t);
                        }
                        else
                        {
                            return new[] {
                                new InvalidBlock(BLOCK_ID, sections).AddMessage(errorMessage)
                            };
                        }
                    }
                    else
                    {
                        return new[] { new InvalidBlock(BLOCK_ID, sections)
                            .AddMessage("Invalid data block.") };
                    }
                }
            }
            return new[] { b };
        }
    }
}
