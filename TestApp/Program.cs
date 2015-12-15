using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swift;

namespace TestApp
{
    class Program
    {
        static void PrintSwift(Swift.Section section, int whiteSpace = 0)
        {
            foreach (var section2 in section.Sections)
            {
                if (whiteSpace != 0)
                {
                    Console.Write(" ".PadLeft(whiteSpace));
                }
                Console.WriteLine(string.Format("{0} => {1}", section2.BlockId, section2.Data));
                PrintSwift(section2, whiteSpace + 5);
            }
        }

        static void Process()
        {
            Swift.Section section;
            using (var r = Swift.Reader.Create("examples\\MT103-72.txt"))
            {
                while ((section = r.Read()) != null)
                {
                    Console.WriteLine("-".PadLeft(30, '-'));
                    PrintSwift(section);

                    var m = GenericMessage.Create(section);

                    var f = m.Text.SwiftFirst("32A");

                    Console.WriteLine(string.Format("32A:Amount:{0}", f.Value.SwiftToDecimal()));
                    Console.WriteLine(string.Format("32A:Currency:{0}", f["Currency"]));
                    Console.WriteLine(string.Format("32A:ValueDate:{0}", f["ValueDate"].SwiftToDateTime()));

                    f = m.Text.SwiftFirst("50K", "50F");
                    Console.WriteLine(string.Format("50?:Account:{0}", f.Value));
                    Console.WriteLine(string.Format("50?:Name:{0}", f["Name"]));
                    Console.WriteLine(string.Format("50?:Address:{0}", f.SwiftConcat(new[] { "Address", "CountryAndTown" })));

                    f = m.Text.SwiftFirstOrDefault("72");
                    if (f != null)
                    {
                        var counter = 0;
                        foreach (var item in f.Where(i => i.Id == "Item"))
                        {
                            Console.WriteLine("72:Item:{0}", ++counter);
                            Console.WriteLine("72:Code:{0}", item["Code"]);
                            Console.WriteLine("72:Narrative:{0}", item.SwiftConcat("Narrative"));
                        }
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            //Task.Yield\
            //var s = new Swift.Settings();
            //Console.WriteLine("<<"+((int)s.End)+">>");
            try
            {
                Process();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }
    }
}
