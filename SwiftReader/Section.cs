using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift
{
    public class Section
    {
        List<Section> _sections;
        string _data;
        string _blockId;

        public int StartPos { get; set; }
        public int EndPos { get; set; }
        public string BlockId
        {
            get
            {
                return _blockId;
            }
            set
            {
                _blockId = value;
            }
        }

        public string Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                if (_data != null)
                {
                    int i = _data.IndexOf(':');
                    if (i > -1)
                    {
                        _blockId = _data.Substring(0, i);
                        _data = (i + 1) < _data.Length ? _data.Substring(i + 1) : string.Empty;
                    }
                    else
                    {
                        _blockId = string.Empty;
                    }
                }
                else
                {
                    _blockId = string.Empty;
                    _data = string.Empty;
                }
            }
        }

        public void Append(string data)
        {
            _data += data;
        }

        public bool NoData
        {
            get
            {
                return string.IsNullOrWhiteSpace(_data);
            }
        }

        public bool HasSections
        {
            get
            {
                return _sections == null && _sections.Count != 0;
            }
        }

        public List<Section> Sections
        {
            get
            {
                if (null == _sections)
                {
                    _sections = new List<Section>();
                }
                return _sections;
            }
        }

        public override string ToString()
        {
            return string.Format("({0}) Start From {1} End To {2} \n {3}", HasSections, StartPos, EndPos, Data);
        }
    }
}
