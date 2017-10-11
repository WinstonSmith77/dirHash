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
                var path = PreparePath(args);

                var xorHashesConten = CalcHash(path);

                Console.WriteLine(xorHashesConten.ToLine());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static string PreparePath(string[] args)
        {
            var path = args[0];

            if (!path.EndsWith(@"\"))
            {
                path = path + "\\";
            }

            return path;
        }

        private static byte[] CalcHash(string path)
        {
            var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);

            var hashesContent =
                files.AsParallel().Select(f => new MD5CryptoServiceProvider().ComputeHash(File.ReadAllBytes(f)));
            var xorHashesContent = hashesContent.XOR();

            var relPathes = files.AsParallel().Select(item => new Uri(path).MakeRelativeUri(new Uri(item)).ToString());
            var xorPathHashes = relPathes
                .Select(p => Encoding.UTF8.GetBytes(p))
                .Select(p => new MD5CryptoServiceProvider().ComputeHash(p))
                .XOR();


            return xorHashesContent.XOR(xorPathHashes);
        }
    }
}
