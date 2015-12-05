using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift
{
    public class Tag : IEnumerable<Tag.TagValue>, IEnumerable
    {
        public class TagValue
        {
            public string Id { get; internal set; }
            public string Value { get; internal set; }
        }

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

        public string Value { get; internal set; }

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
        }

        public string Get(string id, string separator = ", ")
        {
            if (_values == null)
                return null;

            return string.Join(separator, _values.Where(item => item.Id == id).Select(item => item.Value).ToArray());
        }

        public void Add(string id, string value)
        {
            if (_values == null)
                _values = new List<TagValue>();

            _values.Add(new TagValue { Id = id, Value = value });
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
