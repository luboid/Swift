using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift
{
    public class Settings
    {
        public static Settings CreateDefault()
        {
            return new Settings
            {
                Begin = (char)1,
                End = (char)3,
                BufferSize = Reader.READ_BUFFER_SIZE,
                MaxCharactersToStart = 512,
                MaxSections = 8,
                CharSet = new CharSet
                {
                    ContiniusCharSets = new[] {
                        new CharSet.ContiniusCharSet { Begin ='0', End='9' },
                        new CharSet.ContiniusCharSet { Begin ='a', End='z' },
                        new CharSet.ContiniusCharSet { Begin ='A', End='Z' }
                    },
                    // by frequency of using
                    OtherSymbols = new[] { '\r', '\n', '{', '}', '+', ',', '-', '.', ' ', '\'', '(', ')', '/', '?', ':', '=',
                                           '’', '@', '#', '!', '”', '%', '&', '*', ';', '<', '>', '_' }
                }
            };
        }

        public CharSet CharSet { get; set; }
        public char Begin { get; set; }
        public char End { get; set; }
        public int BufferSize { get; set; }
        public int MaxSections { get; set; }
        public int MaxCharactersToStart { get; set; }
    }
}