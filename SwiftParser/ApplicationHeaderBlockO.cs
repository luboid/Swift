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
            new Regex("(O)([0-9]{3})([0-9]{4})([0-9A-Z]{28})([0-9]{6})([0-9]{4})([SNU]{1})", RegexOptions.Compiled);

        public ApplicationHeaderBlockO()
        {
            Type = 'O';
        }

        public string InputTime { get; set; }
        public string InputReference { get; set; }
        public string OutputDate { get; set; }
        public string OutputTime { get; set; }
    }
}
