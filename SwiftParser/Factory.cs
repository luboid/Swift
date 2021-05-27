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

            public string Description { get; internal set; }
            public string Letter { get; internal set; }
            public bool Blob { get; internal set; }
            public int MultiLine { get; internal set; }

            // CounterPostfix remove this at some poit and support SubTag field which can combine multiple values againts one key
            // public bool CounterPostfix { get; internal set; } // adds number to sub field name

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
            public string Description { get; internal set; }
            public TagOption[] Options { get; internal set; }
        }

        static Factory()
        {
            // Header tag
            Tag("42", "Drafts at",
                new TagOption
                {
                    Description = "Drafts at",
                    Letter = "C",
                    Rows = new[]
                    {
                        new TagOptionRow
                        {
                            Regex = new[] { new Regex(@"^(?<Narrative>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled) },
                            Lines = 3,
                            ValueNames = new[] { "Narrative" }
                        }
                    }
                },
                new TagOption
                {
                    Description = "Drawee",
                    Letter = "A",
                    Rows = new[] {
                        new TagOptionRow
                        {
                            Regex = new [] { new Regex(@"^(?<PartyIdentifier>(\/[A-Z]{1,1})?(\/[A-Z0-9a-z\/\-?().,'+: ]{1,34})?)?$", RegexOptions.Compiled) },
                            ValueNames = new [] { "PartyIdentifier" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] { new Regex("^(?<IdentifierCode>[A-Z]{4,4}[A-Z]{2,2}[A-Z0-9]{2,2}([A-Z0-9]{3,3})?)$", RegexOptions.Compiled) },
                            ValueNames = new [] { "IdentifierCode" }
                        }
                    }
                },
                new TagOption
                {
                    Letter = "D",
                    Rows = new[] {
                        new TagOptionRow
                        {
                            Regex = new [] { new Regex(@"^(?<PartyIdentifier>(\/[A-Z]{1,1})?(\/[A-Z0-9a-z\/\-?().,'+: ]{1,34})?)?$", RegexOptions.Compiled) },
                            ValueNames = new [] { "PartyIdentifier" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<NameAddress>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled) },
                            Lines = 4,
                            ValueNames = new[] { "NameAddress" }
                        }
                    }
                },
                new TagOption
                {
                    Description = "Mixed Payment Details",
                    Letter = "M",
                    Rows = new[]
                    {
                        new TagOptionRow
                        {
                            Regex = new[] { new Regex(@"^(?<Narrative>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled) },
                            Lines = 4,
                            ValueNames = new[] { "Narrative" }
                        }
                    }
                },
                new TagOption
                {
                    Description = "Negotiation/Deferred Payment Details",
                    Letter = "P",
                    Rows = new[]
                    {
                        new TagOptionRow
                        {
                            Regex = new[] { new Regex(@"^(?<Narrative>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled) },
                            Lines = 4,
                            ValueNames = new[] { "Narrative" }
                        }
                    }
                }
            );
            Tag("43", "Partial Shipments",
                new TagOption()
                {
                    Description = "Partial Shipments",
                    Letter = "A",
                    Rows = new[] {
                        new TagOptionRow
                        {
                            Regex = new[] { new Regex("^(?<Code>ALLOWED|CONDITIONAL|NOT ALLOWED)$", RegexOptions.Compiled) },
                            ValueNames = new [] { "Code" }
                        }
                    }
                },
                new TagOption()
                {
                    Letter = "T",
                    Description = "Transhipment",
                    Rows = new[] {
                        new TagOptionRow
                        {
                            Regex = new[] { new Regex("^(?<Code>ALLOWED|CONDITIONAL|NOT ALLOWED)$", RegexOptions.Compiled) },
                            ValueNames = new [] { "Code" }
                        }
                    }
                },
                new TagOption()
                {
                    Letter = "P",
                    Description = "Partial Shipments",
                    Rows = new[] {
                        new TagOptionRow
                        {
                            Regex = new[] { new Regex("^(?<Code>ALLOWED|CONDITIONAL|NOT ALLOWED)$", RegexOptions.Compiled) },
                            ValueNames = new [] { "Code" }
                        }
                    }
                }
            );

            Tag("44", "Place of Taking in Charge/Dispatch from .../Place of Receipt",
                new TagOption()
                {
                    Letter = "A",
                    Description = "Place of Taking in Charge/Dispatch from .../Place of Receipt",
                    Rows = new[] {
                        new TagOptionRow
                        {
                            Regex = new[] { new Regex(@"^(?<Narrative>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,65})$", RegexOptions.Compiled) },
                            ValueNames = new[] { "Narrative" }
                        }
                    }
                },
                new TagOption()
                {
                    Letter = "E",
                    Description = "Port of Loading/Airport of Departure",
                    Rows = new[] {
                        new TagOptionRow
                        {
                            Regex = new[] { new Regex(@"^(?<Narrative>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,65})$", RegexOptions.Compiled) },
                            ValueNames = new[] { "Narrative" }
                        }
                    }
                },
                new TagOption()
                {
                    Letter = "F",
                    Description = "Port of Discharge/Airport of Destination",
                    Rows = new[] {
                        new TagOptionRow
                        {
                            Regex = new[] { new Regex(@"^(?<Narrative>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,65})$", RegexOptions.Compiled) },
                            ValueNames = new[] { "Narrative" }
                        }
                    }
                },
                new TagOption()
                {
                    Letter = "B",
                    Description = "Place of Final Destination/For Transportation to .../Place of Delivery",
                    Rows = new[] {
                        new TagOptionRow
                        {
                            Regex = new[] { new Regex(@"^(?<Narrative>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,65})$", RegexOptions.Compiled) },
                            ValueNames = new[] { "Narrative" }
                        }
                    }
                },
                new TagOption()
                {
                    Letter = "C",
                    Description = "Place of Final Destination/For Transportation to .../Place of Delivery",
                    Rows = new[]
                    {
                        new TagOptionRow
                        {
                            Regex = new [] { new Regex(@"^(?<Date>[0-9]{6,6})$", RegexOptions.Compiled) },
                            ValueNames = new [] { "Date" }
                        }
                    }
                },
                new TagOption()
                {
                    Letter = "D",
                    Description = "Shipment Period",
                    Rows = new[] {
                        new TagOptionRow
                        {
                            Regex = new[] { new Regex(@"^(?<Narrative>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,65})$", RegexOptions.Compiled) },
                            Lines = 6,
                            ValueNames = new[] { "Narrative" }
                        }
                    }
                }
            );

            Tag("45", "Description of Goods and/or Services",
                new TagOption()
                {
                    Letter = "A",
                    Description = "Description of Goods and/or Services",
                    Rows = new[]
                    {
                        new TagOptionRow
                        {
                            Regex = new [] {
                                new Regex(@"^(?<Narrative>[A-Z0-9a-z\/\-?().,'+ =:!""%&*<>;{@#_ ]{1,65})$")
                            },
                            ValueNames = new[] { "Narrative" },
                            Lines = 100
                        }
                    }
                },
                new TagOption()
                {
                    Letter = "B",
                    Description = "Description of Goods and/or Services",
                    Rows = new[]
                    {
                        new TagOptionRow
                        {
                            Regex = new [] { new Regex(@"^\/(?<Code>(ADD|DELETE|REPALL))\/(?<Narrative>[A-Z0-9a-z\/\-?().,'+ =:!""%&*<>;{@#_ ]{1,57})$") },
                            ValueNames = new[] { "$,0.agg:Item:Code:Narrative" }
                        },
                        new TagOptionRow
                        {
                            Regex = new [] {
                                new Regex(@"^\/(?<Code>(ADD|DELETE|REPALL))\/(?<Narrative>[A-Z0-9a-z\/\-?().,'+ =:!""%&*<>;{@#_ ]{1,57})$"),
                                new Regex(@"^(?<Narrative>[A-Z0-9a-z\/\-?().,'+ =:!""%&*<>;{@#_ ]{1,65})$")
                            },
                            ValueNames = new[] { "$,0.agg:Item:Code:Narrative,1.agg.prev:Item:Narrative" },
                            Lines = 99
                        }
                    }
                }
            );
            Tag("46", "Documents Required",
                new TagOption()
                {
                    Letter = "A",
                    Description = "Documents Required",
                    Rows = new[]
                    {
                        new TagOptionRow
                        {
                            Regex = new [] {
                                new Regex(@"^(?<Narrative>[A-Z0-9a-z\/\-?().,'+ =:!""%&*<>;{@#_ ]{1,65})$")
                            },
                            ValueNames = new[] { "Narrative" },
                            Lines = 100
                        }
                    }
                },
                new TagOption()
                {
                    Letter = "B",
                    Description = "Documents Required",
                    Rows = new[]
                    {
                        new TagOptionRow
                        {
                            Regex = new [] { new Regex(@"^\/(?<Code>(ADD|DELETE|REPALL))\/(?<Narrative>[A-Z0-9a-z\/\-?().,'+ =:!""%&*<>;{@#_ ]{1,57})$") },
                            ValueNames = new[] { "$,0.agg:Item:Code:Narrative" }
                        },
                        new TagOptionRow
                        {
                            Regex = new [] {
                                new Regex(@"^\/(?<Code>(ADD|DELETE|REPALL))\/(?<Narrative>[A-Z0-9a-z\/\-?().,'+ =:!""%&*<>;{@#_ ]{1,57})$"),
                                new Regex(@"^(?<Narrative>[A-Z0-9a-z\/\-?().,'+ =:!""%&*<>;{@#_ ]{1,65})$")
                            },
                            ValueNames = new[] { "$,0.agg:Item:Code:Narrative,1.agg.prev:Item:Narrative" },
                            Lines = 99
                        }
                    }
                }
            );

            Tag("47", "Additional Conditions",
               new TagOption()
               {
                   Letter = "A",
                   Description = "Additional Conditions",
                   Rows = new[]
                   {
                        new TagOptionRow
                        {
                            Regex = new [] {
                                new Regex(@"^(?<Narrative>[A-Z0-9a-z\/\-?().,'+ =:!""%&*<>;{@#_ ]{1,65})$")
                            },
                            ValueNames = new[] { "Narrative" },
                            Lines = 100
                        }
                   }
               },
               new TagOption()
               {
                   Letter = "B",
                   Description = "Additional Conditions",
                   Rows = new[]
                   {
                        new TagOptionRow
                        {
                            Regex = new [] { new Regex(@"^\/(?<Code>(ADD|DELETE|REPALL))\/(?<Narrative>[A-Z0-9a-z\/\-?().,'+ =:!""%&*<>;{@#_ ]{1,57})$") },
                            ValueNames = new[] { "$,0.agg:Item:Code:Narrative" }
                        },
                        new TagOptionRow
                        {
                            Regex = new [] {
                                new Regex(@"^\/(?<Code>(ADD|DELETE|REPALL))\/(?<Narrative>[A-Z0-9a-z\/\-?().,'+ =:!""%&*<>;{@#_ ]{1,57})$"),
                                new Regex(@"^(?<Narrative>[A-Z0-9a-z\/\-?().,'+ =:!""%&*<>;{@#_ ]{1,65})$")
                            },
                            ValueNames = new[] { "$,0.agg:Item:Code:Narrative,1.agg.prev:Item:Narrative" },
                            Lines = 99
                        }
                   }
               }
           );

            Tag("48", null,
                new TagOption()
                {
                    Letter = "",
                    Description = "Period for Presentation in Days",
                    Rows = new[] {
                        new TagOptionRow
                        {
                            Regex = new [] { new Regex(@"^(?<Days>[0-9]{1,3})(/(?<Narrative>[A-Z0-9a-z\/\-?().,'+ ]{1,35}))?$", RegexOptions.Compiled) },
                            ValueNames = new[] { "Days", "Narrative" }
                        },
                    },
                });

            Tag("49", "",
                new TagOption()
                {
                    Letter = "",
                    Description = "Confirmation Instructions",
                    Rows = new[]
                    {
                        new TagOptionRow
                        {
                            Regex = new[] { new Regex("^(?<Instruction>CONFIRM|MAY ADD|WITHOUT)$", RegexOptions.Compiled) },
                            ValueNames = new[] { "Instruction" }
                        }
                    }

                },
                new TagOption()
                {
                    Letter = "G",
                    Description = "Special Payment Conditions for Beneficiary",
                    Rows = new[]
                    {
                        new TagOptionRow
                        {
                            Regex = new [] {
                                new Regex(@"^(?<Narrative>[A-Z0-9a-z\/\-?().,'+ =:!""%&*<>;{@#_ ]{1,65})$")
                            },
                            ValueNames = new[] { "Narrative" },
                            Lines = 100
                        }
                    }
                },
                new TagOption()
                {
                    Letter = "H",
                    Description = "Special Payment Conditions for Receiving Bank",
                    Rows = new[]
                    {
                        new TagOptionRow
                        {
                            Regex = new [] {
                                new Regex(@"^(?<Narrative>[A-Z0-9a-z\/\-?().,'+ =:!""%&*<>;{@#_ ]{1,65})$")
                            },
                            ValueNames = new[] { "Narrative" },
                            Lines = 100
                        }
                    }
                },
                new TagOption()
                {
                    Letter = "M",
                    Description = "Special Payment Conditions for Beneficiary",
                    Rows = new[]
                    {
                        new TagOptionRow
                        {
                            Regex = new [] { new Regex(@"^\/(?<Code>(ADD|DELETE|REPALL))\/(?<Narrative>[A-Z0-9a-z\/\-?().,'+ =:!""%&*<>;{@#_ ]{1,57})$") },
                            ValueNames = new[] { "$,0.agg:Item:Code:Narrative" }
                        },
                        new TagOptionRow
                        {
                            Regex = new [] {
                                new Regex(@"^\/(?<Code>(ADD|DELETE|REPALL))\/(?<Narrative>[A-Z0-9a-z\/\-?().,'+ =:!""%&*<>;{@#_ ]{1,57})$"),
                                new Regex(@"^(?<Narrative>[A-Z0-9a-z\/\-?().,'+ =:!""%&*<>;{@#_ ]{1,65})$")
                            },
                            ValueNames = new[] { "$,0.agg:Item:Code:Narrative,1.agg.prev:Item:Narrative" },
                            Lines = 99
                        }
                    }
                },
                new TagOption()
                {
                    Letter = "N",
                    Description = "Special Payment Conditions for Receiving Bank",
                    Rows = new[]
                    {
                        new TagOptionRow
                        {
                            Regex = new [] { new Regex(@"^\/(?<Code>(ADD|DELETE|REPALL))\/(?<Narrative>[A-Z0-9a-z\/\-?().,'+ =:!""%&*<>;{@#_ ]{1,57})$") },
                            ValueNames = new[] { "$,0.agg:Item:Code:Narrative" }
                        },
                        new TagOptionRow
                        {
                            Regex = new [] {
                                new Regex(@"^\/(?<Code>(ADD|DELETE|REPALL))\/(?<Narrative>[A-Z0-9a-z\/\-?().,'+ =:!""%&*<>;{@#_ ]{1,57})$"),
                                new Regex(@"^(?<Narrative>[A-Z0-9a-z\/\-?().,'+ =:!""%&*<>;{@#_ ]{1,65})$")
                            },
                            ValueNames = new[] { "$,0.agg:Item:Code:Narrative,1.agg.prev:Item:Narrative" },
                            Lines = 99
                        }
                    }
                }
            );

            Tag("41", "Available With ... By ...",
                new TagOption
                {
                    Letter = "A",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] { new Regex("^(?<IdentifierCode>[A-Z]{4,4}[A-Z]{2,2}[A-Z0-9]{2,2}([A-Z0-9]{3,3})?)$", RegexOptions.Compiled) },
                            ValueNames = new [] { "IdentifierCode" }
                        },
                        new TagOptionRow
                        {
                            Regex = new[] { new Regex("^(?<Code>BY ACCEPTANCE|BY DEF PAYMENT|BY MIXED PYMT|BY NEGOTIATION|BY PAYMENT)$", RegexOptions.Compiled) },
                            ValueNames = new [] { "Code" }
                        }
                    }
                },
                new TagOption
                {
                    Letter = "D",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<Name>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Name" }
                        },
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<Address>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled) },
                            Lines = 3,
                            ValueNames = new[] { "Address" }
                        },
                        new TagOptionRow
                        {
                            Regex = new[] { new Regex("^(?<Code>BY ACCEPTANCE|BY DEF PAYMENT|BY MIXED PYMT|BY NEGOTIATION|BY PAYMENT)$", RegexOptions.Compiled) },
                            ValueNames = new [] { "Code" }
                        }
                    }
                }
            );
            Tag("22", "Purpose of Message",
                new TagOption
                {
                    Letter = "A",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] { new Regex("^(ACNF|ADVI|ISSU)$", RegexOptions.Compiled) }
                        }
                    }
                }
            );

            Tag("40", "Form of Documentary Credit",
                new TagOption
                {
                    Letter = "A",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] { new Regex("^(IRREVOCABLE|IRREVOCABLE TRANSFERABLE|IRREVOCABLE STANDBY|IRREVOC TRANS STANDBY)$", RegexOptions.Compiled) }
                        }
                    }
                },
                new TagOption
                {
                    Description = "Applicable Rules",
                    Letter = "C",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<ApplicableRules>ISPR|NONE|OTHR|URDG)(\/{0,1})(?<Narrative>[A-Z0-9a-z\-?().,'+ ]{1,35})?", RegexOptions.Compiled) },
                            ValueNames = new [] { "ApplicableRules", "Narrative" }
                        }
                    }
                },
                new TagOption
                {
                    Description = "Applicable Rules",
                    Letter = "E",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<ApplicableRules>EUCP LATEST VERSION|EUCPURR LATEST VERSION|ISP LATEST VERSION|OTHR|UCP LATEST VERSION|UCPURR LATEST VERSION)(\/?)(?<Narrative>[A-Z0-9a-z\-?().,'+ ]{1,35})?", RegexOptions.Compiled) },
                            ValueNames = new [] { "ApplicableRules", "Narrative" }
                        }
                    }
                }
            );
            Tag("27", "Sequence of Total", new TagOption()
            {
                Rows = new[]
                {
                    new TagOptionRow
                    {
                        Regex = new [] { new Regex(@"^(?<Number>[0-9]{1,1})/(?<Total>[0-9]{1,1})$", RegexOptions.Compiled) },
                        ValueNames = new [] {"Number", "Total" }
                    }
                }
            });
            Tag("30", "Date", new TagOption()
            {
                Rows = new[]
                {
                    new TagOptionRow
                    {
                        Regex = new [] { new Regex(@"^(?<Date>[0-9]{6,6})$", RegexOptions.Compiled) },
                        ValueNames = new [] { "Date" }
                    }
                }
            });
            Tag("31", "Date of Issue or Request to Issue",
                new TagOption()
                {
                    Letter = "C",
                    Rows = new[]
                    {
                        new TagOptionRow
                        {
                            Regex = new [] { new Regex(@"^(?<Date>[0-9]{6,6})$", RegexOptions.Compiled) },
                            ValueNames = new [] { "Date" }
                        }
                    },
                },
                new TagOption()
                {
                    Letter = "D",
                    Description = "Date and Place of Expiry",
                    Rows = new[]
                    {
                        new TagOptionRow
                        {
                            Regex = new [] { new Regex(@"^(?<Date>[0-9]{6,6})(?<Place>[A-Z0-9a-z\/\-?().,'+: ]{1,29})$", RegexOptions.Compiled) }
                        }
                    },
                }
            );

            Tag("177", "Date&Time of sending", new TagOption
            {
                Letter = "",
                Rows = new[] {
                    new TagOptionRow {
                        Regex = new[] { new Regex(@"^(?<Value>\d{6})(?<Time>\d{4})$", RegexOptions.Compiled) },
                        ValueNames = new[] { "Value", "Time" }
                    }
                }
            });
            Tag("13", "Time Indication",
                new TagOption
                {
                    Letter = "C",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^\/(?<Value>CLSTIME|RNCTIME|SNDTIME|FROTIME|TILTIME)\/(?<TimeIndication>\d{4})(?<TimeOffset>[+-]{1}\d{4})$", RegexOptions.Compiled) },
                            ValueNames = new[] { "Value", "TimeIndication", "TimeOffset" }
                        }
                    }
                },
                new TagOption
                {
                    Letter = "D",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^\/(?<Date>\d{6})(?<Time>\d{4})(?<Offset>[+-]{1}\d{4})$", RegexOptions.Compiled) },
                            ValueNames = new[] { "Date", "Time", "Offset" }
                        }
                    }
                });
            Tag("20", "Sender's Reference", new TagOption
            {
                Rows = new[] {
                    new TagOptionRow {
                        Regex = new[] { new Regex(@"^[A-Za-z0-9/\-?().,'+ ]{1,16}$", RegexOptions.Compiled) } //m.Value
                    }
                }
            });
            Tag("21", "Related Reference", new TagOption
            {
                Rows = new[] {
                    new TagOptionRow {
                        Regex = new[] { new Regex(@"^[A-Za-z0-9/\-?().,'+ ]{1,16}$", RegexOptions.Compiled) } //m.Value
                    }
                }
            });
            Tag("23", "Bank Operation Code",
                new TagOption
                {
                    Description = "Further Identification",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] { new Regex("^(?<Value>ISSUE|REQUEST)$", RegexOptions.Compiled) },
                            ValueNames = new [] { "Value" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^[A-Za-z0-9/\-?().,'+ ]{1,16}$", RegexOptions.Compiled) },
                        }
                    }
                },
                new TagOption
                {
                    Letter = "S",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] { new Regex("^(CANCEL)$", RegexOptions.Compiled) }
                        }
                    }
                },
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
                            Regex = new[] { new Regex(@"^(?<Value>[0-9A-Z]{4})(?<AdditionalInformation>(?:\/)([A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,30}))?$", RegexOptions.Compiled) },
                            ValueNames = new[] { "Value", "AdditionalInformation" }
                        }
                    }
                });
            Tag("25", "Account Identification",
                new TagOption
                {
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled) },
                        }
                    }
                },
                new TagOption
                {
                    Letter = "P",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled) },
                            ValueNames = new[] { "Value" }
                        },
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<IdentifierCode>(?<Bank>[A-Z]{4})(?<Country>[A-Z]{2})(?<Location>[0-9A-Z]{2})(?<Branch>([0-9A-Z]{3}|)))$", RegexOptions.Compiled) },
                            ValueNames = new[] { "IdentifierCode", "Bank", "Country", "Location", "Branch" }
                        }
                    }

                });
            Tag("26", "Transaction Type Code", new TagOption
            {
                Letter = "T",
                Rows = new[] {
                    new TagOptionRow {
                        Regex = new[] { new Regex(@"^([0-9A-Z]{3})$", RegexOptions.Compiled) }//m.Value
                    }
                }
            }, new TagOption
            {
                Description = "Number of Amendment",
                Letter = "E",
                Rows = new[]
                {
                    new TagOptionRow {
                        Regex = new[] { new Regex(@"^[0-9]{1,3}$", RegexOptions.Compiled) }//m.Value
                    }
                }
            });
            Tag("28", "Statement Number/Sequence Number", new TagOption
            {
                Letter = "C",
                Rows = new[] {
                    new TagOptionRow {
                        Regex = new[] { new Regex(@"^(?<Value>[0-9]{1,5})((?:\/)(?<SequenceNumber>[0-9]{1,5}))?$", RegexOptions.Compiled) },
                        ValueNames = new[] { "Value", "SequenceNumber" }
                    }
                }
            });
            Tag("32", "Value Date/Currency/Interbank Settled Amount",
                new TagOption
                {
                    Letter = "A",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<ValueDate>\d{6})(?<Currency>[A-Z]{3})(?<Value>[\d,]{1,15})$", RegexOptions.Compiled) },
                            ValueNames = new[] { "ValueDate", "Currency", "Value" }
                        }
                    }
                },
                new TagOption
                {
                    Description = "Increase of Documentary Credit Amount",
                    Letter = "B",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<Currency>[A-Z]{3})(?<Value>[\d,]{1,15})$", RegexOptions.Compiled) },
                            ValueNames = new[] { "Currency", "Value" }
                        }
                    }
                },
                new TagOption
                {
                    Description = "Amount of Charges",
                    Letter = "D",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<ValueDate>\d{6})(?<Currency>[A-Z]{3})(?<Value>[\d,]{1,15})$", RegexOptions.Compiled) },
                            ValueNames = new[] { "ValueDate", "Currency", "Value" }
                        }
                    }
                }
            );
            Tag("33", "Currency/Instructed Amount",
                new TagOption
                {
                    Letter = "B",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<Currency>[A-Z]{3})(?<Value>[\d,]{1,15})$", RegexOptions.Compiled) },
                            ValueNames = new[] { "Currency", "Value" }
                        }
                    }
                }
            );
            Tag("36", "Exchange Rate", new TagOption
            {
                Rows = new[] {
                    new TagOptionRow {
                        Regex = new[] { new Regex(@"^([\d,]{1,12})$", RegexOptions.Compiled) } //m.Value
                    }
                }
            });
            Tag("39", "Percentage Credit Amount Tolerance",
                new TagOption
                {
                    Letter = "A",
                    Rows = new[] {
                        new TagOptionRow
                        {
                            Regex = new [] {new Regex(@"^(?<Tolerance1>[0-9]{1,2})/(?<Tolerance2>[0-9]{1,2})$", RegexOptions.Compiled) },
                            ValueNames = new [] { "Tolerance1", "Tolerance2" }
                        }
                    }
                },
                new TagOption
                {
                    Letter = "C",
                    Rows = new[] {
                        new TagOptionRow
                        {
                            Regex = new[] { new Regex(@"^(?<Narrative>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled) },
                            Lines = 4,
                            ValueNames = new[] { "Narrative" }
                        }
                    }
                }
            );
            Tag("50", "Ordering Customer",
                new TagOption
                {
                    Description = "Applicant Details",
                    Letter = "",
                    Rows = new[]
                    {
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<NameAddress>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled) },
                            Lines = 4,
                            ValueNames = new[] { "NameAddress" }
                        }
                    }
                },
                new TagOption
                {
                    Letter = "A",
                    Rows = new[] {
                        new TagOptionRow {
                             Regex = new [] {
                                 new Regex(@"^((?:\/{1})(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,34}))$", RegexOptions.Compiled)
                             },
                             ValueNames = new[] { "Value" },
                             Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<IdentifierCode>[A-Z]{4}(?<Country>[A-Z]{2})[0-9A-Z]{2}(?:[0-9A-Z]{3}|))$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "IdentifierCode", "Country" }
                        }
                    }
                },
                new TagOption
                {
                    Letter = "B",
                    Rows = new[]
                    {
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<Name>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Name" }
                        },
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<Address>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled) },
                            Lines = 3,
                            ValueNames = new[] { "Address" }
                        }
                    }
                },
                new TagOption
                {
                    Letter = "F",
                    Rows = new[] {
                        new TagOptionRow {
                             Regex = new [] {
                                 new Regex(@"^((?:\/{1})(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,34}))$", RegexOptions.Compiled), // Mark which option is matched 0,1
                                 new Regex(@"^(?<Code>[A-Z]{4})\/(?<Country>[A-Z]{2})\/(?<Identifier>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,27})$", RegexOptions.Compiled)
                             },
                             ValueNames = new[] { "Value", "Country", "Code", "Identifier" }
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^1\/(?<Name>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,33})$")
                            },
                            ValueNames = new[] { "Name" }
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^1\/(?<Name>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,33})$")
                            },
                            Optional = true,
                            ValueNames = new[] { "Name" }
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<Id>[245678]{1})\/(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,33})$"),
                                new Regex(@"^(?<Id>3)\/(?<Value>((?<Country>[A-Z]{2})((?:\/)(?<Town>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,30}))?)|(?<Town>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,33}))$")
                            },
                            ValueNames = new[] { "$Id:Value,1:Name,2:Address,3:CountryAndTown:Country:Town,4:DateOfBirth,5:PlaceOfBirth,6:CustomerIdNumber,7:NationalIdNumber,8:AdditionalInformation" },
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
                                 new Regex(@"^((?:\/{1})(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,34}))$", RegexOptions.Compiled)
                             },
                             ValueNames = new[] { "Value" },
                             Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<Name>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Name" }
                        },
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<Address>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled) },
                            Lines = 3,
                            ValueNames = new[] { "Address" }
                        }
                    }
                });
            Tag("51", "Sending Institution",
                new TagOption
                {
                    Letter = "A",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?:\/)(?<Value>[A-Z]{1})$", RegexOptions.Compiled),
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,34})$", RegexOptions.Compiled)
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
                        new TagOptionRow
                        {
                            Regex = new [] { new Regex(@"^(?<PartyIdentifier>(\/[A-Z]{1,1})?(\/[A-Z0-9a-z\/\-?().,'+: ]{1,34})?)?$", RegexOptions.Compiled) },
                            ValueNames = new [] { "PartyIdentifier" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<NameAddress>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled) },
                            Lines = 4,
                            ValueNames = new[] { "NameAddress" }
                        }
                    }
                }
            );
            Tag("52", "Ordering Institution",
                new TagOption
                {
                    Letter = "A",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?:\/)(?<Value>[A-Z]{1})$", RegexOptions.Compiled),
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,34})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Value" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<IdentifierCode>[A-Z]{4}(?<Country>[A-Z]{2})[0-9A-Z]{2}(?:[0-9A-Z]{3}|))$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "IdentifierCode", "Country" }
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
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,34})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Value" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<Name>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Name" }
                        },
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<Address>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled) },
                            Lines = 3,
                            ValueNames = new[] { "Address" }
                        }
                    }
                });
            Tag("53", "Sender's Correspondent",
                new TagOption
                {
                    Letter = "A",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?:\/)(?<Value>[A-Z]{1})$", RegexOptions.Compiled),
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,34})$", RegexOptions.Compiled)
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
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,34})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Value" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<Location>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled)
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
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,34})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Value" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<Name>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Name" }
                        },
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<Address>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled) },
                            Lines = 3,
                            ValueNames = new[] { "Address" }
                        }
                    }
                });
            Tag("54", "Receiver's Correspondent",
                new TagOption
                {
                    Letter = "A",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?:\/)(?<Value>[A-Z]{1})$", RegexOptions.Compiled),
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,34})$", RegexOptions.Compiled)
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
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,34})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Value" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<Location>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled)
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
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,34})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Value" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<Name>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Name" }
                        },
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<Address>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled) },
                            Lines = 3,
                            ValueNames = new[] { "Address" }
                        }
                    }
                });
            Tag("55", "Third Reimbursement Institution",
                new TagOption
                {
                    Letter = "A",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?:\/)(?<Value>[A-Z]{1})$", RegexOptions.Compiled),
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,34})$", RegexOptions.Compiled)
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
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,34})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Value" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<Location>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled)
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
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,34})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Value" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<Name>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Name" }
                        },
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<Address>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled) },
                            Lines = 3,
                            ValueNames = new[] { "Address" }
                        }
                    }
                });
            Tag("56", "Intermediary Institution",
                new TagOption
                {
                    Letter = "A",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?:\/)(?<Value>[A-Z]{1})$", RegexOptions.Compiled),
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,34})$", RegexOptions.Compiled)
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
                                new Regex(@"^((?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,34}))$", RegexOptions.Compiled)
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
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,34})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Value" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<Name>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Name" }
                        },
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<Address>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled) },
                            Lines = 3,
                            ValueNames = new[] { "Address" }
                        }
                    }
                });
            Tag("58", "Beneficiary Institution",
                new TagOption
                {
                    Letter = "A",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?:\/)(?<Value>[A-Z]{1})$", RegexOptions.Compiled),
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,34})$", RegexOptions.Compiled)
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
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,34})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Value" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<Name>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Name" }
                        },
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<Address>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled) },
                            Lines = 3,
                            ValueNames = new[] { "Address" }
                        }
                    }
                });
            Tag("57", "Account With Institution",
                new TagOption
                {
                    Letter = "A",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?:\/)(?<Value>[A-Z]{1})$", RegexOptions.Compiled),
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,34})$", RegexOptions.Compiled)
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
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,34})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Value" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<Location>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled)
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
                                new Regex(@"^((?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,34}))$", RegexOptions.Compiled)
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
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,34})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Value" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<Name>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Name" }
                        },
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<Address>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled) },
                            Lines = 3,
                            ValueNames = new[] { "Address" }
                        }
                    }
                });
            Tag("59", "Beneficiary Customer",
                new TagOption
                {
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^((?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,34}))$", RegexOptions.Compiled) },
                            ValueNames = new[] { "Value" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<Name>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled) },
                            ValueNames = new[] { "Name" }
                        },
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<Address>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled) },
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
                                new Regex(@"^(?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,34})$", RegexOptions.Compiled)
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
                                 new Regex(@"^((?:\/)(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,34}))$", RegexOptions.Compiled)
                             },
                             ValueNames = new[] { "Value" },
                             Optional = true
                        },
                        new TagOptionRow {
                            Regex = new[] {
                                new Regex(@"^(?<Id>[1-2]{1})\/(?<Value>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,33})$"),
                                new Regex(@"^(?<Id>3)\/(?<Value>((?<Country>[A-Z]{2})((?:\/)(?<Town>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,30}))?)|(?<Town>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,33}))$")
                            },
                            ValueNames = new[] { "$Id:Value,1:Name,2:Address,3:CountryAndTown:Country:Town" },
                            Lines = 4
                        }
                    }
                });
            Tag("60", "Opening Balance", new TagOption
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
            Tag("61", "Statement Line", new TagOption
            {
                Rows = new[] {
                    new TagOptionRow {
                        Regex = new[] { new Regex(@"^(?<ValueDate>[0-9]{6})(?<EntryDate>[0-9]{4})?(?<Mark>[R]{0,1}[DC]{1})(?<FundCode>[A-Z]{1})?(?<Value>[\d,]{1,15})(?<TransactionType>[A-Z]{1}[0-9A-Z]{3})(((?<AOReference>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,16}(?=\/\/))((?:\/\/)(?<SIReference>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,16}))?)|(?<AOReference>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,16}))$", RegexOptions.Compiled) },
                        ValueNames = new[] { "ValueDate", "EntryDate", "Mark", "FundCode", "Value", "TransactionType", "AOReference", "SIReference" }
                    },
                    new TagOptionRow {
                        Regex = new[] { new Regex(@"^(?<Details>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled) },
                        ValueNames = new[] { "Details" },
                        Optional = true
                    }
                }
            });
            Tag("62", "Closing Balance (Booked Funds)", new TagOption
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
            Tag("64", "Closing Available Balance (Available Funds)", new TagOption
            {
                Rows = new[] {
                    new TagOptionRow {
                        Regex = new[] { new Regex(@"^(?<Mark>[DC]{1})(?<Date>\d{6})(?<Currency>[A-Z]{3})(?<Value>[\d,]{1,15})$", RegexOptions.Compiled) },
                        ValueNames = new[] { "Value", "Mark", "Date", "Currency" }
                    }
                }
            });
            Tag("70", "Remittance Information", new TagOption
            {
                Rows = new[] {
                    new TagOptionRow {
                        Regex = new[] { new Regex(@"^(?<Narrative>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled) },
                        Lines = 4,
                        ValueNames = new[] { "Narrative" }
                    }
                }
            });
            Tag("71", null,
                new TagOption
                {
                    Description = "Details of Charges",
                    Letter = "A",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<Value>BEN|OUR|SHA)$", RegexOptions.Compiled) }
                        }
                    }
                },
                new TagOption
                {
                    Description = "Sender's Charges",
                    Letter = "F",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<Currency>[A-Z]{3})(?<Value>[\d,]{1,15})$", RegexOptions.Compiled) },
                            ValueNames = new[] { "Currency", "Value" }
                        }
                    }
                },
                new TagOption
                {
                    Description = "Receiver's Charges",
                    Letter = "G",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^(?<Currency>[A-Z]{3})(?<Value>[\d,]{1,15})$", RegexOptions.Compiled) },
                            ValueNames = new[] { "Currency", "Value" }
                        }
                    }
                },
                new TagOption
                {
                    Description = "Details of Charges",
                    Letter = "B",
                    Rows = new[]
                    {
                        new TagOptionRow
                        {
                            Regex = new []
                            {
                                new Regex(@"^\/(?<Code>[A-Z]{1,8})\/(?<Currency>[A-Z]{0,3})(?<Amount>[0-9,]{0,13})(?<Narrative>[A-Z0-9a-z\/\-?().,'+: ]{0,9})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "$,0.agg:Item:Code:Currency:Amount:Narrative" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Lines = 5,
                            Regex = new [] {
                                new Regex(@"^\/(?<Code>[A-Z]{1,8})\/(?<Currency>[A-Z]{0,3})(?<Amount>[0-9,]{0,13})(?<Narrative>[A-Z0-9a-z\/\-?().,'+: ]{0,9})$", RegexOptions.Compiled),
                                new Regex(@"^\/\/(?<Narrative>[A-Z0-9a-z\/\-?().,'+: ]{1,35})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "$,0.agg:Item:Code:Currency:Amount:Narrative,1.agg.prev:Item:Narrative" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Lines = 6,
                            Regex = new [] {
                                new Regex(@"^(?<Narrative>[A-Z0-9a-z\/\-?().,'+: ]{1,35})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Narrative" }
                        }
                    }
                },
                new TagOption
                {
                    Description = "Details of Charges",
                    Letter = "N",
                    Rows = new[]
                    {
                        new TagOptionRow
                        {
                            Regex = new []
                            {
                                new Regex(@"^(?<Code>APPL|BENE|OTHR)$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Code" }
                        },
                        new TagOptionRow {
                            Lines = 6,
                            Regex = new [] {
                                new Regex(@"(?<Narrative>[A-Z0-9a-z\/\-?().,'+ =:!""%&*<>;{@#_ ]{1,35})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Narrative" },
                            Optional = true
                        }
                    }
                },
                new TagOption
                {
                    Description = "Charges",
                    Letter = "D",
                    Rows = new[]
                    {
                        new TagOptionRow
                        {
                            Regex = new []
                            {
                                new Regex(@"^\/(?<Code>AGENT|COMM|CORCOM|DISC|INSUR|POST|STAMP|TELECHAR|WAREHOUS)\/(?<Currency>[A-Z]{3,3})(?<Amount>[0-9,]{1,13})(?<Narrative>[A-Z0-9a-z\/\-?().,'+ =:!""%&*<>;{@#_ ]{1,9})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "$,0.agg:Item:Code:Currency:Amount:Narrative" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Lines = 5,
                            Regex = new [] {
                                new Regex(@"^\/(?<Code>AGENT|COMM|CORCOM|DISC|INSUR|POST|STAMP|TELECHAR|WAREHOUS)\/(?<Currency>[A-Z]{3,3})(?<Amount>[0-9,]{1,13})(?<Narrative>[A-Z0-9a-z\/\-?().,'+ =:!""%&*<>;{@#_ ]{1,9})$", RegexOptions.Compiled),
                                new Regex(@"(?<Narrative>[A-Z0-9a-z\/\-?().,'+ =:!""%&*<>;{@#_ ]{1,35})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "$,0.agg:Item:Code:Currency:Amount:Narrative,1.agg.prev:Item:Narrative" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Lines = 6,
                            Regex = new [] {
                                new Regex(@"(?<Narrative>[A-Z0-9a-z\/\-?().,'+ =:!""%&*<>;{@#_ ]{1,35})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "Narrative" },
                            Optional = true
                        }
                    }
                }
            );
            Tag("72", "Sender to Receiver Information",
                new TagOption
                {
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new [] { new Regex(@"^\/(?<Code>[0-9A-Z]{1,8})\/(?<Narrative>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,32})?$", RegexOptions.Compiled) },
                            ValueNames = new[] { "$,0.agg:Item:Code:Narrative" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Lines = 5,
                            Regex = new [] {
                                new Regex(@"^\/(?<Code>[0-9A-Z]{1,8})\/(?<Narrative>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,32})?$", RegexOptions.Compiled),
                                new Regex(@"^\/\/(?<Narrative>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,33})$", RegexOptions.Compiled),
                            },
                            ValueNames = new[] { "$,0.agg:Item:Code:Narrative,1.agg.prev:Item:Narrative" },
                            Optional = true
                        },
                            new TagOptionRow
                        {
                            Lines = 6,
                            Regex = new [] {
                                new Regex(@"^(?<Narrative>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled)
                            },
                            ValueNames = new [] { "Narrative" }
                        }
                    }
                },
                new TagOption
                {
                    Letter = "Z",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new [] { new Regex(@"^\/(?<Code>[0-9A-Z]{1,8})\/(?<Narrative>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,32})?$", RegexOptions.Compiled) },
                            ValueNames = new[] { "$,0.agg:Item:Code:Narrative" },
                            Optional = true
                        },
                        new TagOptionRow {
                            Lines = 5,
                            Regex = new [] {
                                new Regex(@"^\/(?<Code>[0-9A-Z]{1,8})\/(?<Narrative>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,32})?$", RegexOptions.Compiled),
                                new Regex(@"^(?<Narrative>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled)
                            },
                            ValueNames = new[] { "$,0.agg:Item:Code:Narrative,1.agg.prev:Item:Narrative" },
                            Optional = true
                        },
                        new TagOptionRow
                        {
                            Lines = 6,
                            Regex = new [] {
                                new Regex(@"^(?<Narrative>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35})$", RegexOptions.Compiled)
                            },
                            ValueNames = new [] { "Narrative" }
                        }
                    }
                }
            );

            Tag("77", "Regulatory Reporting",
                new TagOption
                {
                    Letter = "B",
                    Rows = new[] {
                        new TagOptionRow {
                            Regex = new[] { new Regex(@"^((\/(?<Value>[A-Z]{1,8})\/(?<Country>[A-Z]{2})((?:\/\/)(?<Narrative>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,29}))?)|(?<Narrative>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35}))$", RegexOptions.Compiled) },
                            ValueNames = new[] { "Value", "Country", "Narrative" }
                        },
                        new TagOptionRow {
                            Lines = 2,
                            Regex = new[] { new Regex(@"^(((?:\/\/)(?<Narrative>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,33}))|(?<Narrative>[A-Za-z0-9\/\-\?\:\(\)\.,'\+ ]{1,35}))$", RegexOptions.Compiled) },
                            ValueNames = new[] { "Narrative" }
                        }
                    }
                },
                new TagOption
                { // blob
                    Letter = "T",
                    Blob = true,
                    Description = "Envelope Contents"
                },
                new TagOption
                {
                    Letter = "C",
                    Rows = new[] {
                        new TagOptionRow
                        {
                            Lines = 150,
                            Regex = new [] {
                                new Regex(@"^(?<Narrative>[A-Z0-9a-z\/\-?().,'+: ]{1,65})$", RegexOptions.Compiled),
                            },
                            ValueNames = new[] { "Narrative" }
                        }
                    },
                    Blob = true,
                    Description = "Amendment Details"
                });

            Tag("78", "Instructions to the Paying/Accepting/Negotiating Bank",
               new TagOption
               {
                   Letter = "",
                   Rows = new[] {
                        new TagOptionRow {
                            Lines = 12,
                            Regex = new[] { new Regex(@"^(?<Narrative>[A-Z0-9a-z\/\-?().,'+: ]{1,65})$", RegexOptions.Compiled) },
                            ValueNames = new[] { "Narrative" }
                        }
                   }
               }
            );

            Tag("79", "",
                new TagOption
                {
                    Description = "Narrative",
                    Letter = "",
                    Rows = new[]
                    {
                        new TagOptionRow {
                            Lines = 35,
                            Regex = new Regex[] { new Regex(@"^(?<Narrative>[A-Z0-9a-z\/\-?().,'+: ]{1,50}$)", RegexOptions.Compiled) },
                            ValueNames = new [] { "Narrative" }
                        }
                    }
                },
                new TagOption
                {
                    Description = "Narrative",
                    Letter = "Z",
                    Rows = new[]
                    {
                        new TagOptionRow {
                            Lines = 35,
                            Regex = new Regex[] { new Regex(@"^(?<Narrative>[A-Z0-9a-z\/\-?().,'+ =:!\""%&*<>;{@#_ ]{1,50}$)", RegexOptions.Compiled) },
                            ValueNames = new [] { "Narrative" }
                        }
                    }
                }
            );
            #region mt9xx
            Message("900", new TagEnabled
            {
                Id = "20",
                Letters = new[] { "" }
            }, new TagEnabled
            {
                Id = "21",
                Letters = new[] { "" }
            }, new TagEnabled
            {
                Id = "25",
                Letters = new[] { "", "P" }
            }, new TagEnabled
            {
                Id = "13",
                Letters = new[] { "D" },
                Optional = true
            }, new TagEnabled
            {
                Id = "32",
                Letters = new[] { "A" }
            }, new TagEnabled
            {
                Id = "52",
                Letters = new[] { "A", "D" },
                Optional = true
            }, new TagEnabled
            {
                Id = "72",
                Letters = new[] { "" },
                Optional = true
            });

            Message("910", new TagEnabled
            {
                Id = "20",
                Letters = new[] { "" }
            }, new TagEnabled
            {
                Id = "21",
                Letters = new[] { "" }
            }, new TagEnabled
            {
                Id = "25",
                Letters = new[] { "", "P" }
            }, new TagEnabled
            {
                Id = "13",
                Letters = new[] { "D" },
                Optional = true
            }, new TagEnabled
            {
                Id = "32",
                Letters = new[] { "A" }
            }, new TagEnabled
            {
                Id = "50",
                Letters = new[] { "A", "F", "K" },
                Optional = true
            }, new TagEnabled
            {
                Id = "52",
                Letters = new[] { "A", "D" },
                Optional = true
            }, new TagEnabled
            {
                Id = "56",
                Letters = new[] { "A", "D" },
                Optional = true
            }, new TagEnabled
            {
                Id = "72",
                Letters = new[] { "" },
                Optional = true
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
            #endregion

            #region mt1xx or mt2xx
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

            Message("202", new TagEnabled
            {
                Id = "20",
                Letters = new[] { "" }
            }, new TagEnabled
            {
                Id = "21",
                Letters = new[] { "" }
            }, new TagEnabled
            {
                Id = "13",
                Letters = new[] { "C" },
                Optional = true
            }, new TagEnabled
            {
                Id = "32",
                Letters = new[] { "A" }
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
                Id = "56",
                Letters = new[] { "A", "D" },
                Optional = true
            }, new TagEnabled
            {
                Id = "57",
                Letters = new[] { "A", "B", "D" },
                Optional = true
            }, new TagEnabled
            {
                Id = "58",
                Letters = new[] { "A", "D" }
            }, new TagEnabled
            {
                Id = "72",
                Letters = new[] { "" },
                Optional = true
            });

            Message("202COV", new TagEnabled
            {
                Id = "20",
                Letters = new[] { "" }
            }, new TagEnabled
            {
                Id = "21",
                Letters = new[] { "" }
            }, new TagEnabled
            {
                Id = "13",
                Letters = new[] { "C" },
                Optional = true
            }, new TagEnabled
            {
                Id = "32",
                Letters = new[] { "A" }
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
                Id = "56",
                Letters = new[] { "A", "D" },
                Optional = true
            }, new TagEnabled
            {
                Id = "57",
                Letters = new[] { "A", "B", "D" },
                Optional = true
            }, new TagEnabled
            {
                Id = "58",
                Letters = new[] { "A", "D" }
            }, new TagEnabled
            {
                Id = "72",
                Letters = new[] { "" },
                Optional = true
            }, new TagEnabled
            {
                Id = "50",
                Letters = new[] { "A", "F", "K" }
            }, new TagEnabled
            {
                Id = "52",
                Letters = new[] { "A", "D" },
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
                Id = "72",
                Letters = new[] { "" },
                Optional = true
            }, new TagEnabled
            {
                Id = "33",
                Letters = new[] { "B" },
                Optional = true
            });
            #endregion

            #region mt7xx

            Message("760",
                new TagEnabled() { Id = "27", Letters = new[] { "" } },
                new TagEnabled() { Id = "20", Letters = new[] { "" } },
                new TagEnabled() { Id = "23", Letters = new[] { "" } },
                new TagEnabled() { Id = "30", Letters = new[] { "" }, Optional = true },
                new TagEnabled() { Id = "40", Letters = new[] { "C" } },
                new TagEnabled() { Id = "77", Letters = new[] { "C" } },
                new TagEnabled() { Id = "72", Letters = new[] { "" }, Optional = true }
            );

            Message("767",
                new TagEnabled() { Id = "27", Letters = new[] { "" } },
                new TagEnabled() { Id = "20", Letters = new[] { "" } },
                new TagEnabled() { Id = "21", Letters = new[] { "" } },
                new TagEnabled() { Id = "23", Letters = new[] { "" } },
                new TagEnabled() { Id = "30", Letters = new[] { "" }, Optional = true },
                new TagEnabled() { Id = "26", Letters = new[] { "E" }, Optional = true },
                new TagEnabled() { Id = "31", Letters = new[] { "C" } },
                new TagEnabled() { Id = "77", Letters = new[] { "C" } },
                new TagEnabled() { Id = "72", Letters = new[] { "" }, Optional = true }
            );

            Message("768",
                new TagEnabled() { Id = "20", Letters = new[] { "" } },
                new TagEnabled() { Id = "21", Letters = new[] { "" } },
                new TagEnabled() { Id = "25", Letters = new[] { "" }, Optional = true },
                new TagEnabled() { Id = "30", Letters = new[] { "" } },
                new TagEnabled() { Id = "32", Letters = new[] { "B", "D" }, Optional = true },
                new TagEnabled() { Id = "57", Letters = new[] { "A", "B", "D" }, Optional = true },
                new TagEnabled() { Id = "71", Letters = new[] { "B" }, Optional = true },
                new TagEnabled() { Id = "72", Letters = new[] { "" }, Optional = true }
            );

            Message("700",
                new TagEnabled() { Id = "27", Letters = new[] { "" } },
                new TagEnabled() { Id = "40", Letters = new[] { "A" } },
                new TagEnabled() { Id = "20", Letters = new[] { "" } },
                new TagEnabled() { Id = "23", Letters = new[] { "" }, Optional = true },
                new TagEnabled() { Id = "31", Letters = new[] { "C" } },
                new TagEnabled() { Id = "40", Letters = new[] { "E" } },
                new TagEnabled() { Id = "31", Letters = new[] { "D" } },
                new TagEnabled() { Id = "51", Letters = new[] { "A", "D" }, Optional = true },
                new TagEnabled() { Id = "50", Letters = new[] { "" } },
                new TagEnabled() { Id = "59", Letters = new[] { "" } },
                new TagEnabled() { Id = "32", Letters = new[] { "B" } },
                new TagEnabled() { Id = "39", Letters = new[] { "A" }, Optional = true },
                new TagEnabled() { Id = "39", Letters = new[] { "C" }, Optional = true },
                new TagEnabled() { Id = "41", Letters = new[] { "A", "D" } },
                new TagEnabled() { Id = "42", Letters = new[] { "C" }, Optional = true },
                new TagEnabled() { Id = "42", Letters = new[] { "A", "D" }, Optional = true },
                new TagEnabled() { Id = "42", Letters = new[] { "M" }, Optional = true },
                new TagEnabled() { Id = "42", Letters = new[] { "P" }, Optional = true },
                new TagEnabled() { Id = "43", Letters = new[] { "P" }, Optional = true },
                new TagEnabled() { Id = "43", Letters = new[] { "T" }, Optional = true },
                new TagEnabled() { Id = "44", Letters = new[] { "A" }, Optional = true },
                new TagEnabled() { Id = "44", Letters = new[] { "E" }, Optional = true },
                new TagEnabled() { Id = "44", Letters = new[] { "F" }, Optional = true },
                new TagEnabled() { Id = "44", Letters = new[] { "B" }, Optional = true },
                new TagEnabled() { Id = "44", Letters = new[] { "C" }, Optional = true },
                new TagEnabled() { Id = "44", Letters = new[] { "D" }, Optional = true },

                new TagEnabled() { Id = "45", Letters = new[] { "A" }, Optional = true },
                new TagEnabled() { Id = "46", Letters = new[] { "A" }, Optional = true },
                new TagEnabled() { Id = "47", Letters = new[] { "A" }, Optional = true },
                new TagEnabled() { Id = "49", Letters = new[] { "G" }, Optional = true },
                new TagEnabled() { Id = "49", Letters = new[] { "H" }, Optional = true },


                new TagEnabled() { Id = "71", Letters = new[] { "D" }, Optional = true },
                new TagEnabled() { Id = "48", Letters = new[] { "" }, Optional = true },
                new TagEnabled() { Id = "49", Letters = new[] { "" } },
                new TagEnabled() { Id = "58", Letters = new[] { "A", "D" }, Optional = true },
                new TagEnabled() { Id = "53", Letters = new[] { "A", "D" }, Optional = true },
                new TagEnabled() { Id = "78", Letters = new[] { "" }, Optional = true },
                new TagEnabled() { Id = "57", Letters = new[] { "A", "B", "D" }, Optional = true },
                new TagEnabled() { Id = "72", Letters = new[] { "Z" }, Optional = true }
            );

            Message("701",
                new TagEnabled() { Id = "27", Letters = new[] { "" } },
                new TagEnabled() { Id = "20", Letters = new[] { "" } },
                new TagEnabled() { Id = "45", Letters = new[] { "A" }, Optional = true },
                new TagEnabled() { Id = "46", Letters = new[] { "A" }, Optional = true },
                new TagEnabled() { Id = "47", Letters = new[] { "A" }, Optional = true },
                new TagEnabled() { Id = "49", Letters = new[] { "G" }, Optional = true },
                new TagEnabled() { Id = "49", Letters = new[] { "H" }, Optional = true }
            );


            Message("707",
                new TagEnabled() { Id = "27", Letters = new[] { "" } },
                new TagEnabled() { Id = "20", Letters = new[] { "" } },
                new TagEnabled() { Id = "21", Letters = new[] { "" } },
                new TagEnabled() { Id = "23", Letters = new[] { "" } },
                new TagEnabled() { Id = "52", Letters = new[] { "A", "D" }, Optional = true },
                new TagEnabled() { Id = "31", Letters = new[] { "C" } },
                new TagEnabled() { Id = "26", Letters = new[] { "E" } },
                new TagEnabled() { Id = "30", Letters = new[] { "" } },
                new TagEnabled() { Id = "22", Letters = new[] { "A" } },
                new TagEnabled() { Id = "23", Letters = new[] { "S" }, Optional = true },
                new TagEnabled() { Id = "40", Letters = new[] { "A" }, Optional = true },
                new TagEnabled() { Id = "40", Letters = new[] { "E" }, Optional = true },
                new TagEnabled() { Id = "31", Letters = new[] { "D" }, Optional = true },
                new TagEnabled() { Id = "50", Letters = new[] { "" }, Optional = true },
                new TagEnabled() { Id = "59", Letters = new[] { "" }, Optional = true },
                new TagEnabled() { Id = "32", Letters = new[] { "B" }, Optional = true },
                new TagEnabled() { Id = "33", Letters = new[] { "B" }, Optional = true },
                new TagEnabled() { Id = "39", Letters = new[] { "A" }, Optional = true },
                new TagEnabled() { Id = "39", Letters = new[] { "C" }, Optional = true },
                new TagEnabled() { Id = "41", Letters = new[] { "A", "D" }, Optional = true },
                new TagEnabled() { Id = "42", Letters = new[] { "A", "D" }, Optional = true },
                new TagEnabled() { Id = "42", Letters = new[] { "C" }, Optional = true },
                new TagEnabled() { Id = "42", Letters = new[] { "M" }, Optional = true },
                new TagEnabled() { Id = "42", Letters = new[] { "P" }, Optional = true },
                new TagEnabled() { Id = "43", Letters = new[] { "P" }, Optional = true },
                new TagEnabled() { Id = "43", Letters = new[] { "T" }, Optional = true },

                new TagEnabled() { Id = "44", Letters = new[] { "A" }, Optional = true },
                new TagEnabled() { Id = "44", Letters = new[] { "E" }, Optional = true },
                new TagEnabled() { Id = "44", Letters = new[] { "F" }, Optional = true },
                new TagEnabled() { Id = "44", Letters = new[] { "B" }, Optional = true },
                new TagEnabled() { Id = "44", Letters = new[] { "C" }, Optional = true },
                new TagEnabled() { Id = "44", Letters = new[] { "D" }, Optional = true },

                new TagEnabled() { Id = "45", Letters = new[] { "B" }, Optional = true },
                new TagEnabled() { Id = "46", Letters = new[] { "B" }, Optional = true },
                new TagEnabled() { Id = "47", Letters = new[] { "B" }, Optional = true },
                new TagEnabled() { Id = "49", Letters = new[] { "M" }, Optional = true },
                new TagEnabled() { Id = "49", Letters = new[] { "N" }, Optional = true },

                new TagEnabled() { Id = "71", Letters = new[] { "D" }, Optional = true },
                new TagEnabled() { Id = "71", Letters = new[] { "N" }, Optional = true },

                new TagEnabled() { Id = "48", Letters = new[] { "" }, Optional = true },
                new TagEnabled() { Id = "49", Letters = new[] { "" }, Optional = true },

                new TagEnabled() { Id = "58", Letters = new[] { "A", "D" }, Optional = true },
                new TagEnabled() { Id = "53", Letters = new[] { "A", "D" }, Optional = true },

                new TagEnabled() { Id = "78", Letters = new[] { "" }, Optional = true },

                new TagEnabled() { Id = "57", Letters = new[] { "A", "B", "D" }, Optional = true },
                new TagEnabled() { Id = "72", Letters = new[] { "Z" }, Optional = true }

            );

            Message("708",
                new TagEnabled() { Id = "27", Letters = new[] { "" } },
                new TagEnabled() { Id = "20", Letters = new[] { "" } },
                new TagEnabled() { Id = "23", Letters = new[] { "" } },
                new TagEnabled() { Id = "26", Letters = new[] { "E" } },
                new TagEnabled() { Id = "30", Letters = new[] { "" } },

                new TagEnabled() { Id = "45", Letters = new[] { "B" }, Optional = true },
                new TagEnabled() { Id = "46", Letters = new[] { "B" }, Optional = true },
                new TagEnabled() { Id = "47", Letters = new[] { "B" }, Optional = true },
                new TagEnabled() { Id = "49", Letters = new[] { "M" }, Optional = true },
                new TagEnabled() { Id = "49", Letters = new[] { "N" }, Optional = true }
            );

            Message("730",
                new TagEnabled() { Id = "20", Letters = new[] { "" } },
                new TagEnabled() { Id = "21", Letters = new[] { "" } },
                new TagEnabled() { Id = "25", Letters = new[] { "" }, Optional = true },
                new TagEnabled() { Id = "30", Letters = new[] { "" } },
                new TagEnabled() { Id = "32", Letters = new[] { "B", "D" }, Optional = true },
                new TagEnabled() { Id = "57", Letters = new[] { "A", "D" }, Optional = true },
                new TagEnabled() { Id = "71", Letters = new[] { "D" }, Optional = true },
                new TagEnabled() { Id = "72", Letters = new[] { "Z" }, Optional = true },
                new TagEnabled() { Id = "79", Letters = new[] { "Z" }, Optional = true }
            );

            Message("799",
                new TagEnabled() { Id = "20", Letters = new[] { "" } },
                new TagEnabled() { Id = "21", Letters = new[] { "" }, Optional = true },
                new TagEnabled() { Id = "79", Letters = new[] { "" } }
            );
            #endregion
        }

        static void Tag(string id, string description, params TagOption[] optios)
        {
            tags[id] = new TagConfig { Id = id, Options = optios, Description = description };
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
                },
                {
                    "202", (msg) => {
                        var cov = msg.User?["119"].Value??string.Empty;
                        if (cov != "COV")
                        {
                            cov=string.Empty;
                        }
                        return msg.App.MessageType + cov;
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

        public static TagOption Tag(string tag, string letter = null)
        {
            TagConfig cfg; letter = letter ?? string.Empty;
            if (tags.TryGetValue(tag, out cfg))
                return cfg.Options.Where(o => o.Letter == letter).FirstOrDefault();
            else
                return null;
        }

        public static string GetDescription(Field field)
        {
            return GetDescription(field.FieldId, field.Letter);
        }

        public static string GetDescription(string tag, string letter)
        {
            TagConfig cfg; letter = letter ?? string.Empty;
            if (tags.TryGetValue(tag, out cfg))
            {
                var t = cfg.Options.Where(o => o.Letter == letter).FirstOrDefault();
                return cfg.Description ?? t?.Description;
            }
            return null;
        }
    }
}