using System.Threading.Tasks;

namespace P2P_File_Transfer_Library
{
    public class Upload
    {
        //Checked in 60328 bytes
        public static async Task Send(string file, string ipaddress, int port = 2222)
        {
            byte[] fbyte = Shared.GetFileByte(file);

            await SendByte(fbyte,ipaddress,port);
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