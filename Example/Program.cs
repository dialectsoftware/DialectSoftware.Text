using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DialectSoftware.Text;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Generating random text with 10,000,000 characters");
            string text = BlackBox.GenerateCode(10000000, BlackBox.CodeGeneratorType.All);
            Console.WriteLine("Building index of text with 10,000,000 characters this should take ~ 1min...");
            var d1 = DateTime.Now;
            BurrowsWheelerTransform bwt = text;
            Console.Write("Building index completed {0} min(s)\r\n\r\nType a phrase to search:", DateTime.Now.Subtract(d1).TotalMinutes);

            string value = null;
            while ("exit" != (value = Console.ReadLine()))
            {
                Console.WriteLine("Searching for {0}...", value);
                d1 = DateTime.Now;
                var results = bwt.Find(value);
                Console.Write("Search completed {0} sec\r\n", DateTime.Now.Subtract(d1).TotalSeconds);
                results.ToList().ForEach(i =>
                {
                    var item = text.Substring(i, value.Length);
                    Debug.Assert(item == value);
                    Console.WriteLine("{0} {1}", i, item);
                });
                Console.WriteLine("{0} results\r\n", results.Count());

                int y = 0;
                d1 = DateTime.Now;
                while (y < text.Length && y > -1)
                {
                    y = text.IndexOf(value, y + (value.Length - 1), StringComparison.Ordinal);
                    Console.WriteLine(y);
                }
                Console.Write("IndexOf completed {0} sec\r\n", DateTime.Now.Subtract(d1).TotalSeconds);

                Console.Write("Type a phrase to search:");

            }

            Console.WriteLine("Serializing index...");
            d1 = DateTime.Now;
            BinaryFormatter b = new BinaryFormatter();
            using(var file = System.IO.File.Create(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "ConsoleApplication2.bin")))
            {
                b.Serialize(file, bwt);
                file.Close();
            }
            Console.WriteLine("Serialization completed {0} sec\r\n", DateTime.Now.Subtract(d1).TotalSeconds);

            Console.WriteLine("Deserializing index...");
            d1 = DateTime.Now;
            using (var file = System.IO.File.Open(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "ConsoleApplication2.bin"),FileMode.Open))
            {
                bwt = (BurrowsWheelerTransform)b.Deserialize(file);
                file.Close();
            }

            Console.Write("Deserialization completed {0} sec\r\nType a phrase to search:", DateTime.Now.Subtract(d1).TotalSeconds);
            while ("exit" != (value = Console.ReadLine()))
            {
                Console.WriteLine("Searching for {0}...", value);
                d1 = DateTime.Now;
                var results = bwt.Find(value);
                Console.Write("Search completed {0} sec\r\n", DateTime.Now.Subtract(d1).TotalSeconds);
                results.ToList().ForEach(i =>
                {
                    var item = text.Substring(i, value.Length);
                    Debug.Assert(item == value);
                    Console.WriteLine("{0} {1}", i, item);
                });
                Console.WriteLine("{0} results\r\n", results.Count());
                Console.Write("Type a phrase to search:");

            }

            Console.ReadLine();
        }
    }
}
