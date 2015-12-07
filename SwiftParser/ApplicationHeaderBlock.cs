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
                        DeliveryMonitoring = grps["DeliveryMonitoring"].Value[0],
                        ObsolescencePeriod = grps["ObsolescencePeriod"].Value
                    } };
                }
            }
            return new[] { new InvalidBlock(BLOCK_ID, sections).AddMessage("Invalid pattern.") };
        }
    }
}
