using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift
{
    public class ApplicationHeaderBlock : IBaseBlock
    {
        public static readonly string BLOCK_ID = "2";

        public char Type { get; protected set; }
        public string MessageType { get; set; }
        public char Priority { get; set; }

        public string BlockId
        {
            get
            {
                return BLOCK_ID;
            }
        }

        public static IBaseBlock[] Create(Section[] sections)
        {
            if (1 != sections.Length)
            {
                return new[] {
                    new InvalidBlock(BLOCK_ID, sections).AddMessage("Message can have only one section of this type.")
                };
            }

            var data = sections[0].Data;
            var match = ApplicationHeaderBlockO.Pattern.Match(data);
            if (match.Success)
            {
                var grps = match.Groups;
                return new[] { new ApplicationHeaderBlockO
                {
                    MessageType = grps[2].Value,
                    InputTime = grps[3].Value,
                    InputReference = grps[4].Value,
                    OutputDate = grps[5].Value,
                    OutputTime = grps[6].Value,
                    Priority = grps[7].Value[0]
                } };
            }
            else
            {
                match = ApplicationHeaderBlockI.Pattern.Match(data);
                if (match.Success)
                {
                    var grps = match.Groups;
                    return new[] { new ApplicationHeaderBlockI
                    {
                        MessageType = grps[2].Value,
                        ReceiverAddress = grps[3].Value,
                        Priority = grps[4].Value[0],
                        DeliveryMonitoring = grps[5].Value[0],
                        ObsolescencePeriod = grps[6].Value
                    } };
                }
            }
            return new[] { new InvalidBlock(BLOCK_ID, sections).AddMessage("Invalid pattern.") };
        }
    }
}
