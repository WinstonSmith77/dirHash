using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dirHash
{
    public static class XORStuff
    {
        public static byte[] XOR(this IEnumerable<byte[]> list)
        {
            return list.Aggregate((acc, item) => acc == null ? item : acc.XOR(item));
        }


        public static byte[] XOR(this byte[] left, byte[] right)
        {
            if (left.Length != right.Length)
            {
                throw new Exception("Error: left and right arrays lengths must be equal.");
            }

            return left.Zip(right, (a, b) => (byte)(a ^ b)).ToArray();
        }

        public static string ToLine(this byte[] left)
        {
            return left.Aggregate("", (acc, item) => acc + " " + item.ToString("X2"));
        }
    }
}
