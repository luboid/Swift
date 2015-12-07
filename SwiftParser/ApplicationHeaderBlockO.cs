using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Swift
{
    public class ApplicationHeaderBlockO : ApplicationHeaderBlock
    {
        public static readonly Regex Pattern = 
            new Regex("(O)(?<MessageType>[0-9]{3})(?<InputTime>[0-9]{4})(?<InputReference>([0-9A-Z]{6})(?<SendersAddress>[0-9A-Z]{12})([0-9A-Z]{10}))(?<OutputDate>[0-9]{6})(?<OutputTime>[0-9]{4})(?<Priority>[SNU]{1})", RegexOptions.Compiled);

        public ApplicationHeaderBlockO()
        {
            Type = 'O';
        }

        public string InputTime { get; set; }
        public string InputReference { get; set; }
        public string OutputDate { get; set; }
        public string OutputTime { get; set; }
        public string SendersAddress { get; set; }
    }
}
