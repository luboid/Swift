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
                    new InvalidBlock(BLOCK_ID, sections)
                    .AddMessage(string.Format("Message can have only one block of this type /{0}/.", BLOCK_ID))
                };
            }

            var data = sections[0].Data;
            var match = ApplicationHeaderBlockO.Pattern.Match(data);
            if (match.Success)
            {
                var grps = match.Groups;
                return new[] { new ApplicationHeaderBlockO
                {
                    MessageType = grps["MessageType"].Value,
                    InputTime = grps["InputTime"].Value,
                    InputReference = grps["InputReference"].Value,
                    OutputDate = grps["OutputDate"].Value,
                    OutputTime = grps["OutputTime"].Value,
                    Priority = grps["Priority"].Value[0],
                    SendersAddress = grps["SendersAddress"].Value
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
                        MessageType = grps["MessageType"].Value,
                        ReceiversAddress = grps["ReceiversAddress"].Value,
                        Priority = grps["Priority"].Value[0],
                        DeliveryMonitoring = grps["DeliveryMonitoring"].Success ? grps["DeliveryMonitoring"].Value[0] : default(char),
                        ObsolescencePeriod = grps["ObsolescencePeriod"].Success ? grps["ObsolescencePeriod"].Value : default(string)
                    } };
                }
            }
            return new[] { new InvalidBlock(BLOCK_ID, sections)
                .AddMessage(string.Format("Invalid pattern of block #{0}, message #{1}.", BLOCK_ID, sections)) };
        }
    }
}
