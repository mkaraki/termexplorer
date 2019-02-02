using System;
using System.Collections.Generic;
using System.Text;

namespace P2P_File_Transfer_Library
{
    public class Shared
    {
        public static string GetBase64(byte[] bytes)
            => Convert.ToBase64String(bytes);

        public static byte[] GetBytesFromBase64(string Base64)
            => Convert.FromBase64String(Base64);

        public static string GetHash(byte[] original)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

            byte[] bs = md5.ComputeHash(original);

            md5.Clear();
            
            string res = BitConverter.ToString(bs).ToLower().Replace("-", "");

            return res;
        }

        public static byte[] GetFileByte(string FilePath)
        {
            if (!System.IO.File.Exists(FilePath))
                throw new System.IO.FileNotFoundException();

            byte[] bytes;
            System.IO.FileStream fs;

            fs = new System.IO.FileStream(FilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            bytes = new byte[fs.Length];
            fs.Read(bytes, 0, (int)fs.Length);
            fs.Close();

            return bytes;
        }

        public static string GetFileHash(string file)
        {
            return GetHash(GetFileByte(file));
        }
    }
}
