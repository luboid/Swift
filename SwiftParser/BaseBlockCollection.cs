using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift
{
    public abstract class BaseBlockCollection<T> : IBaseBlock, IEnumerable<T>
        where T : Tag
    {
        List<T> _list;

        protected abstract T New(string id);

        public abstract string BlockId
        {
            get;
        }

        internal protected virtual List<T> List
        {
            get
            {
                if (null == _list)
                    _list = new List<T>();
                return _list;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return List.GetEnumerator();
        }

        public bool ContainsKey(string key)
        {
            if (null == _list)
                return false;

            return _list.Where(item => item.Id == key).Any();
        }

        public T this[string key]
        {
            get
            {
                if (null == _list)
                    return New(key);

                return _list.Where(item => item.Id == key).FirstOrDefault() ?? New(key);
            }
        }
    }
}
