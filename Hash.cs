using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace dirHash
{
    public static class Hash
    {
        public static byte[] CalcHash(this byte[] data)
        {
            return new MD5CryptoServiceProvider().ComputeHash(data);
        }
    }
}
