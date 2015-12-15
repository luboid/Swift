using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift
{
    public class TagValue : IEnumerable<TagValue.TagValueItem>
    {
        public class TagValueItem
        {
            public string Id { get; internal set; }
            public string Value { get; internal set; }
        }

        List<TagValueItem> _values;

        public string Id { get; internal set; }
        public string Value
        {
            get
            {
                return this["Value"];
            }
            internal set
            {
                this["Value"] = value;
            }
        }

        public string this[string id]
        {
            get
            {
                if (_values == null)
                    return null;

                return _values.Where(item => item.Id == id).FirstOrDefault()?.Value;
            }
            internal protected set
            {
                var item = Values.Where(v => v.Id == id).FirstOrDefault();
                if (null == item)
                {
                    Add(id, value);
                }
                else
                {
                    item.Value = value;
                }
            }
        }

        internal List<TagValueItem> Values
        {
            get
            {
                if (null == _values)
                    _values = new List<TagValueItem>();
                return _values;
            }
        }

        internal protected void Add(string id, string value)
        {
            Values.Add(new TagValueItem { Id = id, Value = value });
        }

        public IEnumerator<TagValueItem> GetEnumerator()
        {
            if (null == _values)
                return Enumerable.Empty<TagValueItem>().GetEnumerator();

            return ((IEnumerable<TagValueItem>)_values).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
