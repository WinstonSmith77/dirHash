using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace dirHash
{
    static class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var start = DateTime.Now;
                var path = PrepareRoot(args);
                var xorHashesConten = CalcHash(path);

                Console.WriteLine("MD5" + xorHashesConten.ToLine());
                Console.WriteLine("Total Seconds " + (DateTime.Now - start).TotalSeconds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static string PrepareRoot(string[] args)
        {
            var path = args[0];

            if (!path.EndsWith(@"\"))
            {
                path = path + "\\";
            }

            return path;
        }

        private static byte[] CalcHash(string root)
        {
            var files = Directory.GetFiles(root, "*", SearchOption.AllDirectories);
           
            var allHashes = files
                .Select(path => CalcHash(root, path))
                .XOR();

            return allHashes;
        }

        private static byte[] CalcHash(string root, string path)
        {
            return HashFromPath(path, root)
                .XOR(HashFromContent(path));
        }

        private static byte[] HashFromContent(string file)
        {
            return File.ReadAllBytes(file).CalcHash();
        }

        private static byte[] HashFromPath(string path, string root)
        {
            var rootPath = CalcRelativePath(root, path);
            return Encoding.UTF8.GetBytes(rootPath).CalcHash();
        }

        private static string CalcRelativePath(string root, string path)
        {
            return new Uri(root).MakeRelativeUri(new Uri(path)).ToString();
        }
    }
}
