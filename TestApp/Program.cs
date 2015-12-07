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
            using (var r = Swift.Reader.Create("examples\\0060692808311101.tgt"))
            {
                while ((section = r.Read()) != null)
                {
                    Console.WriteLine("-".PadLeft(30, '-'));
                    PrintSwift(section);

                    var m = GenericMessage.Create(section);

                    var f = m.Text.SwiftFirst("32A");

                    Console.WriteLine(string.Format("Amount:{0}", f.Value.SwiftToDecimal()));
                    Console.WriteLine(string.Format("Currency:{0}", f["Currency"]));
                    Console.WriteLine(string.Format("ValueDate:{0}", f["ValueDate"].SwiftToDateTime()));

                    f = m.Text.SwiftFirst("50K", "50F");
                    Console.WriteLine(string.Format("Account:{0}", f.Value));
                    Console.WriteLine(string.Format("Name:{0}", f["Name"]));
                    Console.WriteLine(string.Format("Address:{0}", f.SwiftConcat(new[] { "Address", "CountryAndTown" })));
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
