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
            Swift.Section section; InvalidBlock[] invalidBlocks; GenericMessage message;
            using (var r = Swift.Reader.Create("examples\\0060692808311101.tgt"))
            {
                while ((section = r.Read()) != null)
                {
                    Console.WriteLine("-".PadLeft(30, '-'));
                    PrintSwift(section);

                    if (GenericMessage.Create(section, out message, out invalidBlocks))
                    {
                        var text = message.OfType<TextBlock>().First();
                        var fields = text.OfType<Field>().ToList();
                        var amount = fields.Where(f => f.Id == "32A").First();
                        Console.WriteLine(string.Format("Amount:{0}", SwiftUtil.ToDecimal(amount.Value)));
                        Console.WriteLine(string.Format("Currency:{0}", amount["Currency"]));
                        Console.WriteLine(string.Format("ValueDate:{0}", SwiftUtil.ToDateTime(amount["ValueDate"])));
                        // do some thining
                    }
                    else
                    {
                        foreach (var b in invalidBlocks)
                            foreach (var m in b.Messages)
                                Console.WriteLine(m);
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
