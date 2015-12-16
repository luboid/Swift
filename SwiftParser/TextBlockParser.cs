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

            public List<string> RawData
            {
                get { return _rawData; }
            }

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
            static string[] zero = new string[0];

            public class NameMapItem
            {
                public string ValueKey { get; set; }
                public string[] Others { get; set; }
                public bool Aggregate { get; set; }
                public bool Previous { get; set; }
            }

            public bool ByIndex { get; set; }
            public string Id { get; set; }
            public string Value { get; set; }
            public Dictionary<string, NameMapItem> KeyName { get; set; }

            public string GetId(Match m)
            {
                var idValue = m.Groups[Id].Value;
                NameMapItem idName;
                if (KeyName.TryGetValue(idValue, out idName))
                    return idName.ValueKey;
                else
                    return "UnknownId";
            }

            public string[] GetOthers(Match m)
            {
                var idValue = m.Groups[Id].Value;
                NameMapItem idName;
                if (KeyName.TryGetValue(idValue, out idName))
                    return idName.Others;
                else
                    return zero;
            }

            public string[] GetOthers(int index)
            {
                NameMapItem idName;
                if (KeyName.TryGetValue(index.ToString(), out idName))
                    return idName.Others;
                else
                    return zero;
            }

            public bool Grouping(int index)
            {
                NameMapItem idName;
                return KeyName.TryGetValue(index.ToString(), out idName) && idName.Aggregate;
            }

            public string GroupingKey(int index)
            {
                NameMapItem idName;
                if (KeyName.TryGetValue(index.ToString(), out idName))
                    return idName.ValueKey;
                else
                    return null;
            }

            public bool Previous(int index)
            {
                NameMapItem idName;
                return KeyName.TryGetValue(index.ToString(), out idName) && idName.Previous;
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

        public static bool ParseHeader(Tag tag, string data)
        {
            var tagDefinition = Factory.Tag(tag.Id);
            if (tagDefinition != null)
            {
                var regEx = tagDefinition.Rows[0].Regex[0];
                var names = tagDefinition.Rows[0].ValueNames;
                var m = regEx.Match(data);
                if (m.Success)
                {
                    if (null == names || 0 == names.Length)
                    {
                        tag.Value = m.Value;
                    }
                    else
                    {
                        foreach (var n in names)
                        {
                            var g = m.Groups[n];
                            if (g.Success)
                            {
                                tag[n] = g.Value;
                            }
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                tag.Value = data;
            }
            return true;
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
                var lineIndex = 0; NameMap map = null; var lines = item.Lines;
                var options = item.TagOption; var tag = item.Tag; TagValue grouping; TagValue previous;

                tag.Raw = item.Data;

                // item.TagOption.CounterPostfix remove this at some poit and support SubTag field which can combine multiple values againts one key
                if (options.Blob)
                {
                    tag.Value = item.Data;
                    continue;
                }
                else
                {
                    grouping = null; previous = null;
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
                            for (int idx = 0, len = r.Regex.Length; idx < len; idx++)
                            {
                                m = r.Regex[idx].Match(line);
                                if (m.Success)
                                {
                                    if (map == null)
                                    {
                                        if (r.ValueNames == null || 0 == r.ValueNames.Length)
                                        {
                                            tag.Value = m.Value;
                                        }
                                        else
                                        {
                                            foreach (var id in r.ValueNames)
                                            {
                                                if ((mg = m.Groups[id]).Success)
                                                {
                                                    tag.Add(id, mg.Value);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (map.ByIndex)
                                        {
                                            if (map.Grouping(idx))
                                            {
                                                var groupingKey = map.GroupingKey(idx);
                                                if (map.Previous(idx))
                                                {
                                                    if (previous != null && previous.Id == groupingKey)
                                                    {
                                                        grouping = previous;
                                                    }
                                                }
                                                else
                                                {
                                                    if (grouping == null || grouping.Id != groupingKey)
                                                        grouping = tag.Add(groupingKey);
                                                }
                                            }

                                            foreach (var id in map.GetOthers(idx))
                                            {
                                                if ((mg = m.Groups[id]).Success)
                                                {
                                                    if (null == grouping)
                                                        tag.Add(id, mg.Value);
                                                    else
                                                        grouping.Add(id, mg.Value);
                                                }
                                            }

                                            previous = grouping;
                                            grouping = null;
                                        }
                                        else
                                        {
                                            item.Tag.Add(
                                                map.GetId(m),
                                                map.GetValue(m));

                                            foreach (var id in map.GetOthers(m))
                                            {
                                                if ((mg = m.Groups[id]).Success)
                                                {
                                                    tag.Add(id, mg.Value);
                                                }
                                            }
                                        }
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

        static Dictionary<int, NameMap> maps = new Dictionary<int, NameMap>();
        static NameMap DecodeNames(string names)
        {
            NameMap map;
            if (!maps.TryGetValue(names.GetHashCode(), out map))
            {
                //$Id:Value,1:Name,2:Address,3:CountryAndTown
                //$,0:Item.agg:Code:Narrative,1:Item.agg.prev:Narrative
                string[] item;
                var idName = string.Empty;
                var valueName = string.Empty;
                var items = names.Substring(1).Split(new[] { ',' });
                if (!string.IsNullOrWhiteSpace(items[0]))
                {
                    item = items[0].Split(new[] { ':' });
                    idName = item[0];
                    valueName = item[1];
                }

                var keyName = new Dictionary<string, NameMap.NameMapItem>(); string[] others; string[] options;
                var fromIndex = 0; bool agg;
                for (int i = 1, l = items.Length; i < l; i++)
                {
                    item = items[i].Split(new[] { ':' });
                    options = item[0].Split(new[] { '.' });
                    item[0] = options[0];
                    agg = Array.IndexOf<string>(options, "agg") > -1;

                    fromIndex = agg ? 2 : string.IsNullOrWhiteSpace(idName) ? 1 : 2;

                    others = new string[item.Length - fromIndex];
                    if (others.Length != 0)
                    {
                        Array.Copy(item, fromIndex, others, 0, item.Length - fromIndex);
                    }

                    keyName.Add(item[0], new NameMap.NameMapItem
                    {
                        ValueKey = agg || !string.IsNullOrWhiteSpace(idName) ? item[1] : null,
                        Others = others,
                        Aggregate = agg,
                        Previous = Array.IndexOf<string>(options, "prev") > -1
                    });
                }
                maps[names.GetHashCode()] = map = new NameMap
                {
                    Id = idName,
                    Value = valueName,
                    KeyName = keyName,
                    ByIndex = string.IsNullOrWhiteSpace(idName) && string.IsNullOrWhiteSpace(idName)
                };
            }
            return map;
        }
    }
}