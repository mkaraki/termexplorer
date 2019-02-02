using System;
using System.Collections.Generic;
using System.Text;

namespace P2P_File_Transfer_Library
{
    class Shared
    {
        public static string GetBase64(byte[] bytes)
            => Convert.ToBase64String(bytes);

        public static byte[] GetBytesFromBase64(string Base64)
            => Convert.FromBase64String(Base64);
    }
}
