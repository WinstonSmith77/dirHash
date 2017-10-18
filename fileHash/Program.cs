using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace fileHash
{
    static class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args == null || args.Length == 0 || !File.Exists(args[0]))
                {
                    throw new ArgumentException("Wrong number or kind of args!");
                }

                var file = File.ReadAllBytes(args[0]);

                var result = new MD5CryptoServiceProvider().ComputeHash(file);

                var output = result.Aggregate("", (acc, item) => acc + " " + item.ToString("X2"));

                Console.WriteLine(output);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
