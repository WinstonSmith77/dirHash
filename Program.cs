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

                Console.WriteLine("MD5" + xorHashesConten.ToLine());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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
                files.AsParallel().Select(HashFromContent);
            var xorHashesContent = hashesContent.XOR();

            var relPathes = files.AsParallel().Select(item => CalcRelativePath(path, item));
            var xorPathHashes = relPathes
                .Select(HashFromPath)
                .XOR();

            return xorHashesContent.XOR(xorPathHashes);
        }

        private static string CalcRelativePath(string root, string path)
        {
            return new Uri(root).MakeRelativeUri(new Uri(path)).ToString();
        }

        private static byte[] HashFromPath(string path)
        {
            return Encoding.UTF8.GetBytes(path).CalcHash();
        }

        private static byte[] HashFromContent(string file)
        {
            return File.ReadAllBytes(file).CalcHash();
        }
    }
}
