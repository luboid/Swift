using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Swift
{
    public static class Factory
    {
        public class TagEnabled
        {
            public string Id { get; internal set; }
            public bool Optional { get; internal set; }
            public string[] Letters { get; internal set; }
        }

        public class TagOptionRow
        {
            public bool Optional { get; internal set; }
            public Regex[] Regex { get; internal set; }
            public string[] ValueNames { get; internal set; }
            public int Lines { get; internal set; }
        }

        public class TagOption
        {
            TagOptionRow[] _rows;

            public TagOption()
            {
                Letter = string.Empty;
                MultiLine = 1;
            }

            public string Letter { get; internal set; }
            public bool Blob { get; internal set; }
            public int MultiLine { get; internal set; }

            // CounterPostfix remove this at some poit and support SubTag field which can combine multiple values againts one key
            public bool CounterPostfix { get; internal set; } // adds number to sub field name

            public TagOptionRow[] Rows
            {
                get
                {
                    return _rows;
                }
                internal set
                {
                    _rows = value;
                    MultiLine = 0;
                    foreach (var r in _rows)
                    {
                        MultiLine += r.Lines == 0 ? 1 : r.Lines;
                    }
                }
            }
        }

        public class TagConfig
        {
            public string Id { get; internal set; }
            public TagOption[] Options { get; internal set; }
        }

        static Factory()
        {
            Tag("13", new TagOption
            {
                Letter = "C",
                Rows = new[] {
                    new TagOptionRow {
                        Regex = new[] { new Regex(@"^\/(?<Value>CLSTIME|RNCTIME|SNDTIME)\/(?<TimeIndication>\d{4})(?<TimeOffset>[+-]{1}\d{4})$", RegexOptions.Compiled) },
                        ValueNames = new[] { "Value", "TimeIndication", "TimeOffset" }
                    }
                }
            });
            Tag("20", new TagOption
            {
                Rows = new[] {
                    new TagOptionRow {
                        Regex = new[] { new Regex(@"^[A-Z0-9/\-?().,'+ ]{1,16}$", RegexOptions.Compiled) } //m.Value
                    }
                }
            });
            Tag("23",
                new TagOption
                {
                    Letter = "B",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] { new Regex("^(CRED|CRTS|SPAY|SPRI|SSTD)$", RegexOptions.Compiled) }//m.Value
                        }
                    }
                },
                new TagOption
                {
                    Letter = "E",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<Value>[0-9A-Z]{4})(?<AdditionalInformation>(?:\/)([A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,30}))?$", RegexOptions.Compiled) },
                            ValueNames = new[] { "Value", "AdditionalInformation" }
                        }
                    }
                });
            Tag("25", new TagOption
            {
                Rows = new[] {
                    new TagOptionRow {
                        Regex = new[] { new Regex(@"^(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,35})$", RegexOptions.Compiled) },
                    }
                }
            });
            Tag("26", new TagOption
            {
                Letter = "T",
                Rows = new[] {
                    new TagOptionRow {
                        Regex = new[] { new Regex(@"^([0-9A-Z]{3})$", RegexOptions.Compiled) }//m.Value
                    }
                }
            });
            Tag("28", new TagOption
            {
                Letter = "C",
                Rows = new[] {
                    new TagOptionRow {
                        Regex = new[] { new Regex(@"^(?<Value>[0-9]{1,5})((?:\/)(?<SequenceNumber>[0-9]{1,5}))?$", RegexOptions.Compiled) },
                        ValueNames = new[] { "Value", "SequenceNumber" }
                    }
                }
            });
            Tag("32", new TagOption
            {
                Letter = "A",
                Rows = new[] {
                    new TagOptionRow {
                        Regex = new[] { new Regex(@"^(?<ValueDate>\d{6})(?<Currency>[A-Z]{3})(?<Value>[\d,]{1,15})$", RegexOptions.Compiled) },
                        ValueNames = new[] { "ValueDate", "Currency", "Value" }
                    }
                }
            });
            Tag("33", new TagOption
            {
                Letter = "B",
                Rows = new[] {
                    new TagOptionRow {
                        Regex = new[] { new Regex(@"^(?<Currency>[A-Z]{3})(?<Value>[\d,]{1,15})$", RegexOptions.Compiled) },
                        ValueNames = new[] { "Currency", "Value" }
                    }
                }
            });
            Tag("36", new TagOption
            {
                Rows = new[] {
                    new TagOptionRow {
                        Regex = new[] { new Regex(@"^([\d,]{1,12})$", RegexOptions.Compiled) } //m.Value
                    }
                }
            });
            Tag("50",
                new TagOption
                {
                    Letter = "A",
                    Rows = new[] {
                        new TagOptionRow {
                             Regex = new [] {
                                 new Regex(@"^((?:\/{1})(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,34}))$", RegexOptions.Compiled)
                             },
                             ValueNames = new[] { "Value" },
                             Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<IdentifierCode>[A-Z]{4}[A-Z]{2}[0-9A-Z]{2}(?:[0-9A-Z]{3}|))$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "IdentifierCode" }
                        }
                    }
                },
                new TagOption
                {
                    Letter = "F",
                    Rows = new[] {
                        new TagOptionRow {
                             Regex = new [] {
                                 new Regex(@"^((?:\/{1})(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,34}))$", RegexOptions.Compiled), // Mark which option is matched 0,1
                                 new Regex(@"^(?<Code>[A-Z]{4})\/(?<Country>[A-Z]{2})\/(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,27})$", RegexOptions.Compiled)
                             },
                             ValueNames = new[] { "Value", "Country", "Code" }
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^1\/(?<Name>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,33})$")
                            },
                            ValueNames = new[] { "Name" }
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<Id>[2-8]{1})\/(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,33})$")
                            },
                            ValueNames = new[] { "$Id:Value,1:Name,2:Address,3:CountryAndTown,4:DateOfBirth,5:PlaceOfBirth,6:CustomerIdNumber,7:NationalIdNumber,8:AdditionalInformation" },
                            Lines = 3
                        }
                    }
                },
                new TagOption
                {
                    Letter = "K",
                    Rows = new[] {
                        new TagOptionRow {
                             Regex = new [] {
                                 new Regex(@"^((?:\/{1})(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,34}))$", RegexOptions.Compiled)
                             },
                             ValueNames = new[] { "Value" },
                             Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<Name>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,35})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Name" }
                        },
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<Address>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,35})$", RegexOptions.Compiled) },
                            Lines = 3,
                            ValueNames = new[] { "Address" }
                        }
                    }
                });
            Tag("51", new TagOption
            {
                Letter = "A",
                Rows = new[] {
                    new TagOptionRow {
                        Regex = new[] {
                            new Regex(@"^(?:\/)(?<Value>[A-Z]{1})$", RegexOptions.Compiled),
                            new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,34})$", RegexOptions.Compiled)
                        },
                        ValueNames = new[] { "Value" },
                        Optional = true
                    },
                    new TagOptionRow {
                        Regex = new[] {
                            new Regex(@"^(?<IdentifierCode>[A-Z]{4}[A-Z]{2}[0-9A-Z]{2}(?:[0-9A-Z]{3}|))$", RegexOptions.Compiled)
                        },
                        ValueNames = new[] { "IdentifierCode" }
                    }
                }
            });
            Tag("52",
                new TagOption
                {
                    Letter = "A",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?:\/)(?<Value>[A-Z]{1})$", RegexOptions.Compiled),
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,34})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Value" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<IdentifierCode>[A-Z]{4}[A-Z]{2}[0-9A-Z]{2}(?:[0-9A-Z]{3}|))$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "IdentifierCode" }
                        }
                    }
                },
                new TagOption
                {
                    Letter = "D",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?:\/)(?<Value>[A-Z]{1})$", RegexOptions.Compiled),
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,34})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Value" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<Name>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,35})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Name" }
                        },
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<Address>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,35})$", RegexOptions.Compiled) },
                            Lines = 3,
                            ValueNames = new[] { "Address" }
                        }
                    }
                });
            Tag("53",
                new TagOption
                {
                    Letter = "A",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?:\/)(?<Value>[A-Z]{1})$", RegexOptions.Compiled),
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,34})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Value" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<IdentifierCode>[A-Z]{4}[A-Z]{2}[0-9A-Z]{2}(?:[0-9A-Z]{3}|))$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "IdentifierCode" }
                        }
                    }
                },
                new TagOption
                {
                    Letter = "B",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?:\/)(?<Value>[A-Z]{1})$", RegexOptions.Compiled),
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,34})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Value" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<Location>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,35})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Location" },
                            Optional = true
                        }
                    }
                },
                new TagOption
                {
                    Letter = "D",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?:\/)(?<Value>[A-Z]{1})$", RegexOptions.Compiled),
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,34})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Value" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<Name>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,35})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Name" }
                        },
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<Address>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,35})$", RegexOptions.Compiled) },
                            Lines = 3,
                            ValueNames = new[] { "Address" }
                        }
                    }
                });
            Tag("54",
                new TagOption
                {
                    Letter = "A",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?:\/)(?<Value>[A-Z]{1})$", RegexOptions.Compiled),
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,34})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Value" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<IdentifierCode>[A-Z]{4}[A-Z]{2}[0-9A-Z]{2}(?:[0-9A-Z]{3}|))$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "IdentifierCode" }
                        }
                    }
                },
                new TagOption
                {
                    Letter = "B",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?:\/)(?<Value>[A-Z]{1})$", RegexOptions.Compiled),
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,34})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Value" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<Location>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,35})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Location" },
                            Optional = true
                        }
                    }
                },
                new TagOption
                {
                    Letter = "D",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?:\/)(?<Value>[A-Z]{1})$", RegexOptions.Compiled),
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,34})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Value" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<Name>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,35})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Name" }
                        },
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<Address>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,35})$", RegexOptions.Compiled) },
                            Lines = 3,
                            ValueNames = new[] { "Address" }
                        }
                    }
                });
            Tag("55",
                new TagOption
                {
                    Letter = "A",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?:\/)(?<Value>[A-Z]{1})$", RegexOptions.Compiled),
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,34})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Value" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<IdentifierCode>[A-Z]{4}[A-Z]{2}[0-9A-Z]{2}(?:[0-9A-Z]{3}|))$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "IdentifierCode" }
                        }
                    }
                },
                new TagOption
                {
                    Letter = "B",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?:\/)(?<Value>[A-Z]{1})$", RegexOptions.Compiled),
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,34})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Value" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<Location>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,35})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Location" },
                            Optional = true
                        }
                    }
                },
                new TagOption
                {
                    Letter = "D",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?:\/)(?<Value>[A-Z]{1})$", RegexOptions.Compiled),
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,34})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Value" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<Name>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,35})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Name" }
                        },
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<Address>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,35})$", RegexOptions.Compiled) },
                            Lines = 3,
                            ValueNames = new[] { "Address" }
                        }
                    }
                });
            Tag("56",
                new TagOption
                {
                    Letter = "A",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?:\/)(?<Value>[A-Z]{1})$", RegexOptions.Compiled),
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,34})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Value" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<IdentifierCode>[A-Z]{4}[A-Z]{2}[0-9A-Z]{2}(?:[0-9A-Z]{3}|))$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "IdentifierCode" }
                        }
                    }
                },
                new TagOption
                {
                    Letter = "C",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^((?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,34}))$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Value" }
                        }
                    }
                },
                new TagOption
                {
                    Letter = "D",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?:\/)(?<Value>[A-Z]{1})$", RegexOptions.Compiled),
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,34})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Value" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<Name>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,35})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Name" }
                        },
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<Address>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,35})$", RegexOptions.Compiled) },
                            Lines = 3,
                            ValueNames = new[] { "Address" }
                        }
                    }
                });
            Tag("57",
                new TagOption
                {
                    Letter = "A",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?:\/)(?<Value>[A-Z]{1})$", RegexOptions.Compiled),
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,34})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Value" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<IdentifierCode>[A-Z]{4}[A-Z]{2}[0-9A-Z]{2}(?:[0-9A-Z]{3}|))$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "IdentifierCode" }
                        }
                    }
                },
                new TagOption
                {
                    Letter = "B",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?:\/)(?<Value>[A-Z]{1})$", RegexOptions.Compiled),
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,34})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Value" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<Location>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,35})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Location" },
                            Optional = true
                        }
                    }
                },
                new TagOption
                {
                    Letter = "C",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^((?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,34}))$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Value" }
                        }
                    }
                },
                new TagOption
                {
                    Letter = "D",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?:\/)(?<Value>[A-Z]{1})$", RegexOptions.Compiled),
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,34})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Value" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<Name>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,35})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Name" }
                        },
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<Address>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,35})$", RegexOptions.Compiled) },
                            Lines = 3,
                            ValueNames = new[] { "Address" }
                        }
                    }
                });
            Tag("59",
                new TagOption
                {
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^((?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,34}))$", RegexOptions.Compiled) },
                            ValueNames = new[] { "Value" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<Name>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,35})$", RegexOptions.Compiled) },
                            ValueNames = new[] { "Name" }
                        },
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<Address>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,35})$", RegexOptions.Compiled) },
                            Lines = 3,
                            ValueNames = new[] { "Address" }
                        }
                    }
                },
                new TagOption
                {
                    Letter = "A",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?:\/)(?<Value>[A-Z]{1})$", RegexOptions.Compiled),
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,34})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Value" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<IdentifierCode>[A-Z]{4}[A-Z]{2}[0-9A-Z]{2}(?:[0-9A-Z]{3}|))$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "IdentifierCode" }
                        }
                    }
                },
                new TagOption
                {
                    Letter = "F",
                    Rows = new[] {
                        new TagOptionRow {
                             Regex = new [] {
                                 new Regex(@"^((?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,34}))$", RegexOptions.Compiled)
                             },
                             ValueNames = new[] { "Value" },
                             Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<Id>[1-3]{1})\/(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,33})$")
                            },
                            ValueNames = new[] { "$Id:Value,1:Name,2:Address,3:CountryAndTown" },
                            Lines = 4
                        }
                    }
                });
            Tag("60", new TagOption
            {
                Letter = "F",
                Rows = new[] {
                    new TagOptionRow {
                        Regex = new[] { new Regex(@"^(?<Mark>[DC]{1})(?<Date>\d{6})(?<Currency>[A-Z]{3})(?<Value>[\d,]{1,15})$", RegexOptions.Compiled) },
                        ValueNames = new[] { "Value", "Mark", "Date", "Currency" }
                    }
                }
            }, new TagOption
            {
                Letter = "M",
                Rows = new[] {
                    new TagOptionRow {
                        Regex = new[] { new Regex(@"^(?<Mark>[DC]{1})(?<Date>\d{6})(?<Currency>[A-Z]{3})(?<Value>[\d,]{1,15})$", RegexOptions.Compiled) },
                        ValueNames = new[] { "Value", "Mark", "Date", "Currency" }
                    }
                }
            });
            Tag("61", new TagOption
            {
                Rows = new[] {
                    new TagOptionRow {
                        Regex = new[] { new Regex(@"^(?<ValueDate>[0-9]{6})(?<EntryDate>[0-9]{4})?(?<Mark>[R]{0,1}[DC]{1})(?<FundCode>[A-Z]{1})?(?<Value>[\d,]{1,15})(?<TransactionType>[A-Z]{1}[0-9A-Z]{3})(((?<AOReference>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,16}(?=\/\/))((?:\/\/)(?<SIReference>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,16}))?)|(?<AOReference>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,16}))$", RegexOptions.Compiled) },
                        ValueNames = new[] { "ValueDate", "EntryDate", "Mark", "FundCode", "Value", "TransactionType", "AOReference", "SIReference" }
                    },
                    new TagOptionRow {
                        Regex = new[] { new Regex(@"^(?<Details>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,35})$", RegexOptions.Compiled) },
                        ValueNames = new[] { "Details" },
                        Optional = true
                    }
                }
            });
            Tag("62", new TagOption
            {
                Letter = "F",
                Rows = new[] {
                    new TagOptionRow {
                        Regex = new[] { new Regex(@"^(?<Mark>[DC]{1})(?<Date>\d{6})(?<Currency>[A-Z]{3})(?<Value>[\d,]{1,15})$", RegexOptions.Compiled) },
                        ValueNames = new[] { "Value", "Mark", "Date", "Currency" }
                    }
                }
            }, new TagOption
            {
                Letter = "M",
                Rows = new[] {
                    new TagOptionRow {
                        Regex = new[] { new Regex(@"^(?<Mark>[DC]{1})(?<Date>\d{6})(?<Currency>[A-Z]{3})(?<Value>[\d,]{1,15})$", RegexOptions.Compiled) },
                        ValueNames = new[] { "Value", "Mark", "Date", "Currency" }
                    }
                }
            });
            Tag("64", new TagOption
            {
                Rows = new[] {
                    new TagOptionRow {
                        Regex = new[] { new Regex(@"^(?<Mark>[DC]{1})(?<Date>\d{6})(?<Currency>[A-Z]{3})(?<Value>[\d,]{1,15})$", RegexOptions.Compiled) },
                        ValueNames = new[] { "Value", "Mark", "Date", "Currency" }
                    }
                }
            });
            Tag("70", new TagOption
            {
                Rows = new[] {
                    new TagOptionRow {
                        Regex = new[] { new Regex(@"^(?<Narrative>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,35})$", RegexOptions.Compiled) },
                        Lines = 4,
                        ValueNames = new[] { "Narrative" }
                    }
                }
            });
            Tag("71",
                new TagOption
                {
                    Letter = "A",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<Value>BEN|OUR|SHA)$", RegexOptions.Compiled) }
                        }
                    }
                },
                new TagOption
                {
                    Letter = "F",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(<Currency>[A-Z]{3})(?<Value>[\d,]{1,15})$", RegexOptions.Compiled) },
                            ValueNames = new[] { "Currency", "Value" }
                        }
                    }
                },
                new TagOption
                {
                    Letter = "G",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(<Currency>[A-Z]{3})(?<Value>[\d,]{1,15})$", RegexOptions.Compiled) },
                            ValueNames = new[] { "Currency", "Value" }
                        }
                    }
                });
            Tag("72", new TagOption
            {
                CounterPostfix = true,
                Rows = new[] {
                    new TagOptionRow {
                        Regex = new [] { new Regex(@"^\/(?<Code>[0-9A-Z]{1,8})\/(?<Narrative>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,32})?$", RegexOptions.Compiled) },
                        ValueNames = new[] { "Code", "Narrative" }
                    },
                    new TagOptionRow {
                        Lines = 5,
                        Regex = new [] {
                            new Regex(@"^\/(?<Code>[0-9A-Z]{1,8})\/(?<Narrative>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,32})?$", RegexOptions.Compiled),
                            new Regex(@"^\/\/(?<Narrative>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,33})$", RegexOptions.Compiled)
                        },
                        ValueNames = new[] { "Code", "Narrative" }
                    }
                }
            });
            Tag("77",
                new TagOption
                {
                    Letter = "B",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^\/(?<Value>[A-Z]{1,8})\/(?<Country>[A-Z]{2})((?:\/\/)(?<Narrative>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,29}))?$", RegexOptions.Compiled) },
                            ValueNames = new[] { "Value", "Country", "Narrative" }
                        },
                        new TagOptionRow {
                            Lines = 2,
                            Regex = new[] { new Regex(@"^(?:\/\/)(?<Narrative>[A-Za-z0-9\/\-\?\:\(\)\.,’\+ ]{1,33})$", RegexOptions.Compiled) },
                            ValueNames = new[] { "Narrative" }
                        }
                    }
                },
                new TagOption
                { // blob
                    Letter = "T",
                    Blob = true
                });

            Message("950", new TagEnabled
            {
                Id = "20",
                Letters = new[] { "" }
            }, new TagEnabled
            {
                Id = "25",
                Letters = new[] { "" }
            }, new TagEnabled
            {
                Id = "28",
                Letters = new[] { "C" }
            }, new TagEnabled
            {
                Id = "60",
                Letters = new[] { "F", "M" }
            }, new TagEnabled
            {
                Id = "61",
                Letters = new[] { "" },
                Optional = true
            }, new TagEnabled
            {
                Id = "62",
                Letters = new[] { "F", "M" }
            }, new TagEnabled
            {
                Id = "64",
                Letters = new[] { "" },
                Optional = true
            });

            Message("103", new TagEnabled
            {
                Id = "20",
                Letters = new[] { "" }
            }, new TagEnabled
            {
                Id = "13",
                Letters = new[] { "C" },
                Optional = true
            }, new TagEnabled
            {
                Id = "23",
                Letters = new[] { "B" }
            }, new TagEnabled
            {
                Id = "23",
                Letters = new[] { "E" },
                Optional = true
            }, new TagEnabled
            {
                Id = "26",
                Letters = new[] { "T" },
                Optional = true
            }, new TagEnabled
            {
                Id = "32",
                Letters = new[] { "A" }
            }, new TagEnabled
            {
                Id = "33",
                Letters = new[] { "B" },
                Optional = true
            }, new TagEnabled
            {
                Id = "36",
                Letters = new[] { "" },
                Optional = true
            }, new TagEnabled
            {
                Id = "50",
                Letters = new[] { "A", "F", "K" }
            }, new TagEnabled
            {
                Id = "51",
                Letters = new[] { "A" },
                Optional = true
            }, new TagEnabled
            {
                Id = "52",
                Letters = new[] { "A", "D" },
                Optional = true
            }, new TagEnabled
            {
                Id = "53",
                Letters = new[] { "A", "B", "D" },
                Optional = true
            }, new TagEnabled
            {
                Id = "54",
                Letters = new[] { "A", "B", "D" },
                Optional = true
            }, new TagEnabled
            {
                Id = "55",
                Letters = new[] { "A", "B", "D" },
                Optional = true
            }, new TagEnabled
            {
                Id = "56",
                Letters = new[] { "A", "C", "D" },
                Optional = true
            }, new TagEnabled
            {
                Id = "57",
                Letters = new[] { "A", "B", "C", "D" },
                Optional = true
            }, new TagEnabled
            {
                Id = "59",
                Letters = new[] { "", "A", "F" }
            }, new TagEnabled
            {
                Id = "70",
                Letters = new[] { "" },
                Optional = true
            }, new TagEnabled
            {
                Id = "71",
                Letters = new[] { "A" }
            }, new TagEnabled
            {
                Id = "71",
                Letters = new[] { "F" },
                Optional = true
            }, new TagEnabled
            {
                Id = "71",
                Letters = new[] { "G" },
                Optional = true
            }, new TagEnabled
            {
                Id = "72",
                Letters = new[] { "" },
                Optional = true
            }, new TagEnabled
            {
                Id = "77",
                Letters = new[] { "B" },
                Optional = true
            });

            Message("103STP", new TagEnabled
            {
                Id = "20",
                Letters = new[] { "" }
            }, new TagEnabled
            {
                Id = "13",
                Letters = new[] { "C" },
                Optional = true
            }, new TagEnabled
            {
                Id = "23",
                Letters = new[] { "B" }
            }, new TagEnabled
            {
                Id = "23",
                Letters = new[] { "E" },
                Optional = true
            }, new TagEnabled
            {
                Id = "26",
                Letters = new[] { "T" },
                Optional = true
            }, new TagEnabled
            {
                Id = "32",
                Letters = new[] { "A" }
            }, new TagEnabled
            {
                Id = "33",
                Letters = new[] { "B" },
                Optional = true
            }, new TagEnabled
            {
                Id = "36",
                Letters = new[] { "" },
                Optional = true
            }, new TagEnabled
            {
                Id = "50",
                Letters = new[] { "A", "F", "K" }
            }, new TagEnabled
            {
                Id = "52",
                Letters = new[] { "A" },
                Optional = true
            }, new TagEnabled
            {
                Id = "53",
                Letters = new[] { "A", "B" },
                Optional = true
            }, new TagEnabled
            {
                Id = "54",
                Letters = new[] { "A" },
                Optional = true
            }, new TagEnabled
            {
                Id = "55",
                Letters = new[] { "A" },
                Optional = true
            }, new TagEnabled
            {
                Id = "56",
                Letters = new[] { "A" },
                Optional = true
            }, new TagEnabled
            {
                Id = "57",
                Letters = new[] { "A" },
                Optional = true
            }, new TagEnabled
            {
                Id = "59",
                Letters = new[] { "", "A", "F" }
            }, new TagEnabled
            {
                Id = "70",
                Letters = new[] { "" },
                Optional = true
            }, new TagEnabled
            {
                Id = "71",
                Letters = new[] { "A" }
            }, new TagEnabled
            {
                Id = "71",
                Letters = new[] { "F" },
                Optional = true
            }, new TagEnabled
            {
                Id = "71",
                Letters = new[] { "G" },
                Optional = true
            }, new TagEnabled
            {
                Id = "72",
                Letters = new[] { "" },
                Optional = true
            }, new TagEnabled
            {
                Id = "77",
                Letters = new[] { "B" },
                Optional = true
            });

            Message("103REMIT", new TagEnabled
            {
                Id = "20",
                Letters = new[] { "" }
            }, new TagEnabled
            {
                Id = "13",
                Letters = new[] { "C" },
                Optional = true
            }, new TagEnabled
            {
                Id = "23",
                Letters = new[] { "B" }
            }, new TagEnabled
            {
                Id = "23",
                Letters = new[] { "E" },
                Optional = true
            }, new TagEnabled
            {
                Id = "26",
                Letters = new[] { "T" },
                Optional = true
            }, new TagEnabled
            {
                Id = "32",
                Letters = new[] { "A" }
            }, new TagEnabled
            {
                Id = "33",
                Letters = new[] { "B" },
                Optional = true
            }, new TagEnabled
            {
                Id = "36",
                Letters = new[] { "" },
                Optional = true
            }, new TagEnabled
            {
                Id = "50",
                Letters = new[] { "A", "F", "K" }
            }, new TagEnabled
            {
                Id = "51",
                Letters = new[] { "A" },
                Optional = true
            }, new TagEnabled
            {
                Id = "52",
                Letters = new[] { "A", "D" },
                Optional = true
            }, new TagEnabled
            {
                Id = "53",
                Letters = new[] { "A", "B", "D" },
                Optional = true
            }, new TagEnabled
            {
                Id = "54",
                Letters = new[] { "A", "B", "D" },
                Optional = true
            }, new TagEnabled
            {
                Id = "55",
                Letters = new[] { "A", "B", "D" },
                Optional = true
            }, new TagEnabled
            {
                Id = "56",
                Letters = new[] { "A", "C", "D" },
                Optional = true
            }, new TagEnabled
            {
                Id = "56",
                Letters = new[] { "A", "B", "C", "D" },
                Optional = true
            }, new TagEnabled
            {
                Id = "59",
                Letters = new[] { "", "A", "F" },
                Optional = true
            }, new TagEnabled
            {
                Id = "71",
                Letters = new[] { "A" }
            }, new TagEnabled
            {
                Id = "71",
                Letters = new[] { "F" },
                Optional = true
            }, new TagEnabled
            {
                Id = "71",
                Letters = new[] { "G" },
                Optional = true
            }, new TagEnabled
            {
                Id = "72",
                Letters = new[] { "" },
                Optional = true
            }, new TagEnabled
            {
                Id = "77",
                Letters = new[] { "B" },
                Optional = true
            }, new TagEnabled
            {
                Id = "77",
                Letters = new[] { "T" }
            });
        }

        static void Tag(string id, params TagOption[] optios)
        {
            tags[id] = new TagConfig { Id = id, Options = optios };
        }

        static void Message(string id, params TagEnabled[] options)
        {
            messages[id] = new List<TagEnabled>(options);
        }

        static Dictionary<string, List<TagEnabled>> messages =
            new Dictionary<string, List<TagEnabled>>();

        static Dictionary<string, TagConfig> tags =
            new Dictionary<string, TagConfig>();

        static Dictionary<string, Func<Section[], IBaseBlock[]>> handlers =
            new Dictionary<string, Func<Section[], IBaseBlock[]>>() {
                { BasicHeaderBlock.BLOCK_ID, BasicHeaderBlock.Create },
                { ApplicationHeaderBlock.BLOCK_ID, ApplicationHeaderBlock.Create },
                { UserHeaderBlock.BLOCK_ID, UserHeaderBlock.Create },
                { TextBlock.BLOCK_ID, TextBlock.Create },
                { TrailerBlock.BLOCK_ID, TrailerBlock.Create },
                { TrailerSBlock.BLOCK_ID, TrailerSBlock.Create }
            };

        static Dictionary<string, Func<GenericMessage, string>> createMessageIdHandlers =
            new Dictionary<string, Func<GenericMessage, string>>() {
                {
                    "103", (msg) => {
                        return msg.App.MessageType + (msg.User?["119"].Value??string.Empty);
                    }
                }
            };


        public static List<TagEnabled> GetMessageFields(string messageTypeId)
        {
            List<TagEnabled> tags;
            if (messages.TryGetValue(messageTypeId, out tags))
                return tags;
            else
                return null;
        }

        public static string CreateMessageId(GenericMessage message)
        {
            Func<GenericMessage, string> handler; var app = message.App;
            if (createMessageIdHandlers.TryGetValue(app?.MessageType, out handler))
                return handler(message);
            else
                return app?.MessageType;
        }

        public static IBaseBlock[] CreateBlock(Section[] sections)
        {
            Func<Section[], IBaseBlock[]> handler; string key = sections[0].BlockId;
            if (handlers.TryGetValue(key, out handler))
                return handler(sections);
            else
                return new[] { new InvalidBlock(key, sections)
                    .AddMessage(string.Format("Can't find handler for section {0}.", key)) };
        }

        public static TagOption Tag(string tag, string letter)
        {
            TagConfig cfg; letter = letter ?? string.Empty;
            if (tags.TryGetValue(tag, out cfg))
                return cfg.Options.Where(o => o.Letter == letter).FirstOrDefault();
            else
                return null;
        }
    }
}