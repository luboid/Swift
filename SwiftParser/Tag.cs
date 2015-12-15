using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift
{
    public class Tag : IEnumerable<TagValue>, IEnumerable
    {
        string _id;
        int _index;
        List<TagValue> _values;

        public Tag() { }

        public Tag(string id, int index)
        {
            _id = id;
            _index = index;
        }

        public string Id { get { return _id; } }
        public int Index { get { return _index; } }

        internal protected List<TagValue> Values
        {
            get
            {
                if (null == _values)
                    _values = new List<TagValue>();
                return _values;
            }
        }

        public bool ContainsKey(string id)
        {
            if (_values == null)
                return false;

            return _values.Where(item => item.Id == id).FirstOrDefault() != null;
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

        public string Value
        {
            get
            {
                return this["Value"];
            }
            internal protected set
            {
                this["Value"] = value;
            }
        }

        internal protected TagValue Add(string id, string value)
        {
            var tagValue = new TagValue { Id = id, Value = value };
            Values.Add(tagValue);
            return tagValue;
        }

        internal protected TagValue Add(string id)
        {
            var tagValue = new TagValue { Id = id };
            Values.Add(tagValue);
            return tagValue;
        }

        public IEnumerator<TagValue> GetEnumerator()
        {
            if (null == _values)
                return System.Linq.Enumerable.Empty<TagValue>().GetEnumerator();

            return ((IEnumerable<TagValue>)_values).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return Id;
        }
    }
}
