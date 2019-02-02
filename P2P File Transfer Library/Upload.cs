using System.Threading.Tasks;

namespace P2P_File_Transfer_Library
{
    public class Upload
    {
        //Checked in 60328 bytes
        public static async Task Send(string file, string ipaddress, int port = 2222)
        {
            byte[] fbyte = GetFileByte(file);

            await SendByte(fbyte,ipaddress,port);
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

        public static async Task SendByte(byte[] bytes,string ipaddress, int port = 2222)
        {
            System.Net.Sockets.UdpClient udp = new System.Net.Sockets.UdpClient();

            try
            {
                await udp.SendAsync(bytes, bytes.Length, ipaddress, port);
            }
            finally
            {
                udp.Close();
            }
        }
    }
}