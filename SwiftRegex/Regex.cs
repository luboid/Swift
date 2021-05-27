using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TextRegex = System.Text.RegularExpressions.Regex;

namespace Swift
{
    public static class Regex
    {
        public static readonly Dictionary<char, string> CharSets = new Dictionary<char, string>();

        static Regex()
        {
            CharSets.Add('A', "A-Z");
            CharSets.Add('N', "0-9");
            CharSets.Add('C', CharSets['A'] + CharSets['N']);
            CharSets.Add('H', CharSets['N'] + "ABCDEF");
            CharSets.Add('D', CharSets['N'] + ",");
            CharSets.Add('E', " ");
            CharSets.Add('X', CharSets['C'] + "a-z" + "/\\-?().,'+ ");
            CharSets.Add('Z', CharSets['X'] + "=:!\"%&*<>;{@#_ ");
        }

        public static string Format(string sSwiftFormatSpec)
        {
            sSwiftFormatSpec = sSwiftFormatSpec?.ToUpper();
            //Split incoming SWIFT format specs into an array on the pipe symbol (pre-existing format)
            var sFormatLines = sSwiftFormatSpec.Split(new char[] { '|' });

            var sRegexLine = "";
            var bAllOptional = true; //Keeps track of whether an entire line is optional
            var bOptional = false; //Keeps track of whether field is optional
            var cCharacterSet = (char)0; //Character set for field
            var sRegex = "^";
            var bNewLineAdded = false;

            foreach (var sLine in sFormatLines)
            {
                bNewLineAdded = false;
                bAllOptional = true;
                sRegexLine = "";

                //Split each line based into fields based on location of the character set code (A,C,D,E,H,N,X or Z).
                //These will be the last character in the set, UNLESS the field is optional in which case it will
                //end with character set code followed by ].  There has to be a cleaner way than replacing then
                //splitting.  Feel free to find it.
                var fields = TextRegex.Replace(sLine, "([ACDEHNXZ]]?)([^\\]])", "$1|$2").Split(new char[] { '|' }); var sField = string.Empty;
                for (int i = 0, l = fields.Length; i < l; i++)
                {
                    sField = fields[i];
                    // Identify the character set
                    cCharacterSet = TextRegex.Match(sField, "[ACDEHNXZ]").Value[0];

                    // Identify optional fields and flag/clean to make following steps easier.
                    if (sField.StartsWith("["))
                    {
                        bOptional = true;
                        sField = sField.Replace("[", "").Replace("]", "");
                    }
                    else
                    {
                        bAllOptional = false;
                        bOptional = false;
                    }

                    //First locate fixed field length indicated by exclamation mark and convert.
                    //e.g. 16!N becomes N{16,16}
                    sField = TextRegex.Replace(sField, @"(\d{1,2})(!)(" + cCharacterSet + ")", "$3{$1,$1}");

                    //Next locate non fixed field length and convert.
                    //e.g. 35X becomes X{1,35}
                    sField = TextRegex.Replace(sField, @"(\d{1,2})(" + cCharacterSet + ")", "$2{1,$1}");

                    //If field is optional then make regex optional.
                    //e.g. [35X] now becomes (X(1,35))?
                    if (bOptional) sField = "(" + sField + ")?";

                    //Lookup the regex conversion of acceptable characters for this SWIFT character
                    //set and replace accordingly.
                    //e.g. 4!N is now [0-9]{4,4}
                    sField = sField.Replace(cCharacterSet.ToString(), "[" + CharSets[cCharacterSet] + "]");

                    //If the field contains a line multiplier repeat the regex as required with
                    //CRLF.
                    //So if regex for 35X is Y then 4*35X becomes (Y\r\n){1,4}
                    if (TextRegex.IsMatch(sField, @"(\d{1,2})(\*)(.*)"))
                    {
                        sField = TextRegex.Replace(sField, @"(\d{1,2})(\*)(.*)", "(^$3\r\n){1,$1}");
                        bNewLineAdded = true;
                    }
                    sRegexLine += sField;
                }

                //Append CRLF if not already there.  This is another hack to make the regex easier.
                if (!bNewLineAdded) sRegexLine += "\r\n";

                //If the entire line consists of only optional fields make the entire line optional in regex
                if (bAllOptional) sRegexLine = "(" + sRegexLine + ")?";
                sRegex += sRegexLine;
            }
            sRegex += @"$";

            return sRegex;
        }
    }
}
