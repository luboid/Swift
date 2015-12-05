using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift
{
    public class GenericMessage : IEnumerable<IBaseBlock>
    {
        List<IBaseBlock> _blocks;

        public Section Raw { get; private set; }
        public BasicHeaderBlock Basic
        {
            get
            {
                return _blocks.OfType<BasicHeaderBlock>().FirstOrDefault();
            }
        }

        public ApplicationHeaderBlock App
        {
            get
            {
                return _blocks.OfType<ApplicationHeaderBlock>().FirstOrDefault();
            }
        }

        public UserHeaderBlock User
        {
            get
            {
                return _blocks.OfType<UserHeaderBlock>().FirstOrDefault();
            }
        }

        public TextBlock Text
        {
            get
            {
                return _blocks.OfType<TextBlock>().FirstOrDefault();
            }
        }

        public TrailerBlock Trailer
        {
            get
            {
                return _blocks.OfType<TrailerBlock>().FirstOrDefault();
            }
        }

        public TrailerSBlock TrailerS
        {
            get
            {
                return _blocks.OfType<TrailerSBlock>().FirstOrDefault();
            }
        }

        public IEnumerator<IBaseBlock> GetEnumerator()
        {
            return ((IEnumerable<IBaseBlock>)_blocks).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static bool Create(Section raw, out GenericMessage message, out InvalidBlock[] invalidBlocks)
        {
            message = null;
            invalidBlocks = null;

            var query = raw.Sections
                .GroupBy(s => s.BlockId)
                .SelectMany(s => Factory.CreateBlock(s.ToArray()));

            invalidBlocks = query
                .OfType<InvalidBlock>()
                .ToArray();

            if (invalidBlocks.Length != 0)
            {
                return false;
            }

            message = new GenericMessage();

            var basic = query
                .OfType<BasicHeaderBlock>()
                .Count();

            if (0 == basic)// only basic block is madatory
            {
                invalidBlocks = new[] {
                    new InvalidBlock("UNKNOWN", new[] { raw })
                        .AddMessage(string.Format("Expect message to have at least one block of type {0}.", BasicHeaderBlock.BLOCK_ID))
                };
            }

            message = new GenericMessage
            {
                Raw = raw,
                _blocks = query.ToList()
            };

            var id = Factory.CreateMessageId(message);
            if (null == id)
            {
                invalidBlocks = new[] {
                    new InvalidBlock("UNKNOWN", new[] { raw })
                    .AddMessage("Unknown message.")
                };
            }

            var tags = Factory.GetMessageFields(id);
            if (null == id)
            {
                invalidBlocks = new[] {
                    new InvalidBlock("UNKNOWN", new[] { raw })
                    .AddMessage(string.Format("Unsupported message {0}.", id))
                };
            }

            var text = message.Text?.OfType<Field>().ToList(); var count = 0;
            if (null != text)
            {
                count = text
                    .Where(a => !tags.Where(t => t.Id == a.FieldId && Array.IndexOf<string>(t.Letters, a.Letter) > -1).Any())
                    .Count();

                if (count > 0)
                {
                    invalidBlocks = new[]
                    {
                        new InvalidBlock("UNKNOWN", new[] { raw })
                            .AddMessage(string.Format("Message contains unsupported fields ({0}).", count))
                    };
                }
            }

            count = tags
                .Where(t => !t.Optional)
                .Where(t => null == text || !text.Where(i => i.FieldId == t.Id && Array.IndexOf(t.Letters, i.Letter) > 0).Any())
                .Count();

            if (count > 0)
            {
                invalidBlocks = new[]
                {
                        new InvalidBlock("UNKNOWN", new[] { raw })
                            .AddMessage(string.Format("Message don't contains mandatory fields ({0}).", count))
                };
            }
            return true;
        }

    }
}