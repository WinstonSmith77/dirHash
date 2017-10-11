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
            if (System.Diagnostics.Debugger.IsAttached)
            {
                Console.ReadLine();
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
            var dir = Directory.GetDirectories(root, "*", SearchOption.AllDirectories);

            var progressInfo = new ProgressInfo(files.Length + dir.Length);
            void Increment() => progressInfo.Increment();

            var allFileHashes = files
                .Select(path => CalcHash(root, path, Increment))
                .XOR();

            var allDirHashes = dir
                .Select(path => HashFromPath(path, root, Increment))
                .XOR();

            return allFileHashes.XOR(allDirHashes);
        }

        private static byte[] CalcHash(string root, string path, Action call)
        {
            var result = HashFromPath(path, root, call)
                .XOR(HashFromContent(path));

            return result;
        }

        private static byte[] HashFromContent(string file)
        {
            return File.ReadAllBytes(file).CalcHash();
        }

        private static byte[] HashFromPath(string path, string root, Action call)
        {
            var rootPath = CalcRelativePath(root, path);
            var result = Encoding.UTF8.GetBytes(rootPath).CalcHash();

            call();

            return result;
        }

        private static string CalcRelativePath(string root, string path)
        {
            return new Uri(root).MakeRelativeUri(new Uri(path)).ToString();
        }
    }
}
