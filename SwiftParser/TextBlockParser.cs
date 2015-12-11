using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Swift.Factory;

namespace Swift
{
    public static class TextBlockParser
    {
        class ParserItem
        {
            List<string> _rawData;

            public List<string> RawData {
                get { return _rawData; } }

            public Field Tag { get; set; }
            public TagOption TagOption { get; set; }

            public bool HasData
            {
                get
                {
                    return _rawData != null && _rawData.Count != 0;
                }
            }

            public int Lines
            {
                get
                {
                    return _rawData == null ? 0 : _rawData.Count;
                }
            }

            public string Data
            {
                get
                {
                    return _rawData == null ? string.Empty : string.Join("\r\n", _rawData);
                }
            }

            public string this[int line]
            {
                get
                {
                    return _rawData == null ? null : _rawData[line];
                }
            }

            public void Append(string data)
            {
                if (null == _rawData)
                    _rawData = new List<string>();
                _rawData.Add(data);
            }
        }

        class NameMap
        {
            public string Id { get; set; }
            public string Value { get; set; }
            public Dictionary<string, string> KeyName { get; set; }

            public string GetId(Match m)
            {
                var idValue = m.Groups[Id].Value;
                string idName;
                if (KeyName.TryGetValue(idValue, out idName))
                    return idName;
                else
                    return "UnknownId";
            }

            public string GetValue(Match m)
            {
                var g = m.Groups[Value];
                return g.Success ? g.Value : null;
            }
        }

        static readonly Regex label = new Regex(":(\\d{2})([A-Z]{0,1}):(.*)", RegexOptions.Compiled);

        public static bool IsDataBlock(Section section)
        {
            if (section.NoData)
            {
                return false;
            }
            else
            {
                var data = section.Data;
                return data.StartsWith("\r\n") && data.EndsWith("\r\n-");
            }
        }

        public static string[] DataBlock(string message)
        {
            return message.Substring(2, message.Length - 5).Split(new[] { "\r\n" }, StringSplitOptions.None);
        }

        public static bool Parse(string data, out string errorMessage, out IList<Tag> tags)
        {
            errorMessage = null;
            tags = null;

            Match m = null; Group mg = null;
            int i = 0, l = 0, index = 0;
            var t = (Factory.TagOption)null; var line = string.Empty;
            var parserItem = (ParserItem)null; var parserItems = new List<ParserItem>();
            var rawLines = DataBlock(data);
            for (i = 0, l = rawLines.Length; i < l; i++)
            {
                line = rawLines[i];
                if (line.StartsWith(":"))
                {
                    m = label.Match(line);
                    if (m.Success)
                    {
                        t = Factory.Tag(m.Groups[1].Value, m.Groups[2].Value);
                        if (null == t)
                        {
                            errorMessage = string.Format("Unknown field {0}{1}.", m.Groups[1].Value, m.Groups[2].Value);
                            return false;
                        }
                        else
                        {
                            parserItem = new ParserItem
                            {
                                TagOption = t,
                                Tag = new Field(m.Groups[1].Value, m.Groups[2].Value, index)
                            };
                            ++index;
                            parserItems.Add(parserItem);
                            line = m.Groups[3].Value;
                        }
                    }
                    else
                    {
                        errorMessage = string.Format("Invalid field definition at line {0}.", i + 1);
                        return false;
                    }
                }

                if (null == parserItem || parserItem.TagOption.MultiLine == parserItem.Lines)
                {
                    errorMessage = string.Format("Expect field definition at the line {0}.", i + 1);
                    return false;
                }
                else
                {
                    parserItem.Append(line);
                }
            }

            foreach (var item in parserItems)
            {
                var lineIndex = 0; NameMap map = null; var lines = item.Lines; var counter = 0;
                var options = item.TagOption; var tag = item.Tag;

                tag.Raw = item.Data;

                // item.TagOption.CounterPostfix remove this at some poit and support SubTag field which can combine multiple values againts one key
                if (options.Blob)
                {
                    tag.Value = item.Data;
                    continue;
                }
                else
                {
                    foreach (var r in options.Rows)
                    {
                        map = null;
                        if (r.ValueNames != null && r.ValueNames[0].StartsWith("$"))
                        {
                            map = DecodeNames(r.ValueNames[0]);
                        }

                        i = 0;
                        do
                        {

                            line = item[lineIndex];
                            foreach (var regEx in r.Regex)
                            {
                                m = regEx.Match(line);
                                if (m.Success)
                                {
                                    if (map == null)
                                    {
                                        if (r.ValueNames == null)
                                        {
                                            tag.Value = m.Value;
                                        }
                                        else
                                        {
                                            foreach (var id in r.ValueNames)
                                            {
                                                if ((mg = m.Groups[id]).Success)
                                                {
                                                    if (id == "Value")
                                                    {
                                                        tag.Value = mg.Value;
                                                    }
                                                    else
                                                    {
                                                        tag.Add(id + (options.CounterPostfix ? counter.ToString() : string.Empty), mg.Value);
                                                    }
                                                }
                                            }
                                            ++counter;
                                        }
                                    }
                                    else
                                    {
                                        item.Tag.Add(
                                            map.GetId(m) + (options.CounterPostfix ? counter.ToString() : string.Empty),
                                            map.GetValue(m));

                                        ++counter;
                                    }
                                    break;
                                }
                            }

                            if (!m.Success && !r.Optional)
                            {
                                errorMessage = string.Format("Field {0} isn't optional.", item.Tag.Id);
                                return false;
                            }

                            if (m.Success)
                            {
                                ++lineIndex;
                                ++i;
                            }
                        }
                        while (i < r.Lines && lineIndex < lines && m.Success);

                        if (lineIndex >= lines) // no more data if regex exists we need to skep them
                            break;
                    }
                }

                if (lineIndex < lines)
                {
                    errorMessage = string.Format("Field {0} can't match data.", item.Tag.Id);
                    return false;
                }
            }

            tags = parserItems
                .Select(item => item.Tag)
                .Cast<Tag>()
                .ToList();

            return true;
        }

        static NameMap DecodeNames(string names)
        {
            //$Id:Value,1:Name,2:Address,3:CountryAndTown
            var items = names.Substring(1).Split(new[] { ',' });
            var item = items[0].Split(new[] { ':' });
            var idName = item[0];
            var valueName = item[1];
            var keyName = new Dictionary<string, string>();
            for (int i = 1, l = items.Length; i < l; i++)
            {
                item = items[i].Split(new[] { ':' });
                keyName.Add(item[0], item[1]);
            }
            return new NameMap { Id = idName, Value = valueName, KeyName = keyName };
        }
    }
}
