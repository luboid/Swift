using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Swift
{
    public class BasicHeaderBlock : IBaseBlock
    {
        public static readonly string BLOCK_ID = "1";
        public static readonly Regex Pattern = new Regex("^([AFL]{1})(01|21)([0-9A-Z]{12})(\\d{4})(\\d{6})$", RegexOptions.Compiled);

        public char ApplicationID { get; set; }
        public string ServiceID { get; set; }
        public string LogicalTerminalAddress { get; set; }
        public string SessionNumber { get; set; }
        public string SequenceNumber { get; set; }

        public string BlockId
        {
            get
            {
                return BLOCK_ID;
            }
        }

        public static IBaseBlock[] Create(Section[] sections)
        {
            List<IBaseBlock> list = new List<IBaseBlock>();
            foreach (var section in sections)
            {
                var match = BasicHeaderBlock.Pattern.Match(section.Data);
                if (match.Success)
                {
                    var grps = match.Groups;
                    list.Add(new BasicHeaderBlock
                    {
                        ApplicationID = grps[1].Value[0],
                        ServiceID = grps[2].Value,
                        LogicalTerminalAddress = grps[3].Value,
                        SessionNumber = grps[4].Value,
                        SequenceNumber = grps[5].Value
                    });
                }
                else
                {
                    return new[] { new InvalidBlock(BLOCK_ID, sections).AddMessage("Invalid pattern.") };
                }
            }
            return list.ToArray();
        }
    }
}
