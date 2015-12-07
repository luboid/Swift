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
        BasicHeaderBlock _basic;
        ApplicationHeaderBlock _app;
        UserHeaderBlock _user;
        TextBlock _text;
        TrailerBlock _trailer;
        TrailerSBlock _trailerS;

        public Section Raw { get; private set; }
        public BasicHeaderBlock Basic
        {
            get
            {
                if (null == _basic)
                {
                    var query = _blocks.OfType<BasicHeaderBlock>().ToList();
                    _basic = query.Where(b => b.ServiceID == "01").FirstOrDefault() ?? query.FirstOrDefault();
                }
                return _basic;
            }
        }

        public ApplicationHeaderBlock App
        {
            get
            {
                if (null == _app)
                {
                    _app = _blocks.OfType<ApplicationHeaderBlock>().FirstOrDefault();
                }
                return _app;
            }
        }

        public UserHeaderBlock User
        {
            get
            {
                if (_user == null)
                {
                    _user = _blocks.OfType<UserHeaderBlock>().FirstOrDefault();
                }
                return _user;
            }
        }

        public TextBlock Text
        {
            get
            {
                if (null == _text)
                {
                    _text = _blocks.OfType<TextBlock>().FirstOrDefault();
                }
                return _text;
            }
        }

        public TrailerBlock Trailer
        {
            get
            {
                if (null == _trailer)
                {
                    _trailer = _blocks.OfType<TrailerBlock>().FirstOrDefault();
                }
                return _trailer;
            }
        }

        public TrailerSBlock TrailerS
        {
            get
            {
                if (null == _trailerS)
                {
                    _trailerS = _blocks.OfType<TrailerSBlock>().FirstOrDefault();
                }
                return _trailerS;
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

        public static GenericMessage Create(Section raw)
        {
            var query = raw.Sections
                .GroupBy(s => s.BlockId)
                .SelectMany(s => Factory.CreateBlock(s.ToArray()));

            var invalidBlocks = query
                .OfType<InvalidBlock>()
                .ToArray();

            if (invalidBlocks.Length != 0)
            {
                throw new InvalidBlockException(invalidBlocks);
            }

            var message = new GenericMessage();

            var basic = query
                .OfType<BasicHeaderBlock>()
                .Count();

            if (0 == basic)// only basic block is madatory
            {
                throw new InvalidBlockException(
                    new InvalidBlock("UNKNOWN", new[] { raw })
                        .AddMessage(string.Format("Expect message to have at least one block of type {0}.", BasicHeaderBlock.BLOCK_ID)));
            }

            message = new GenericMessage
            {
                Raw = raw,
                _blocks = query.ToList()
            };

            var id = Factory.CreateMessageId(message);
            if (null == id)
            {
                throw new InvalidBlockException(new InvalidBlock("UNKNOWN", new[] { raw })
                    .AddMessage("Unknown message."));
            }

            var tags = Factory.GetMessageFields(id);
            if (null == id)
            {
                throw new InvalidBlockException(new InvalidBlock("UNKNOWN", new[] { raw })
                    .AddMessage(string.Format("Unsupported message {0}.", id)));
            }

            var text = message.Text?.OfType<Field>().ToList(); var count = 0;
            if (null != text)
            {
                count = text
                    .Where(a => !tags.Where(t => t.Id == a.FieldId && Array.IndexOf<string>(t.Letters, a.Letter) > -1).Any())
                    .Count();

                if (count > 0)
                {
                    throw new InvalidBlockException(new InvalidBlock(TextBlock.BLOCK_ID, raw.Sections.Where(i => i.BlockId == TextBlock.BLOCK_ID).ToArray())
                            .AddMessage(string.Format("Message contains unsupported fields ({0}).", count)));
                }
            }

            count = tags
                .Where(t => !t.Optional)
                .Where(t => null == text || !text.Where(i => i.FieldId == t.Id && Array.IndexOf(t.Letters, i.Letter) > -1).Any())
                .Count();

            if (count > 0)
            {
                throw new InvalidBlockException(new InvalidBlock(TextBlock.BLOCK_ID, raw.Sections.Where(i => i.BlockId == TextBlock.BLOCK_ID).ToArray())
                            .AddMessage("Message don't contains mandatory fields."));
            }

            // mutual exclusive fields
            count = tags
                .Where(t => t.Letters.Length > 1)
                .Select(t => null == text ? 0 : text.Where(i => i.FieldId == t.Id && Array.IndexOf(t.Letters, i.Letter) > -1).Select(i => i.Id).Distinct().Count())
                .Where(i => i > 1)
                .Count();
            if (count > 0)
            {
                throw new InvalidBlockException(new InvalidBlock(TextBlock.BLOCK_ID, raw.Sections.Where(i => i.BlockId == TextBlock.BLOCK_ID).ToArray())
                            .AddMessage("Message contains mutual exclusive fields."));
            }

            return message;
        }

    }
}