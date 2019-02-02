using System.Net.Sockets;
using System.Threading.Tasks;

namespace P2P_File_Transfer_Library
{
    public class Download
    {
        public static byte[] WaitByte(int port = 2222)
        {
            UdpClient udp = new UdpClient(AddressFamily.InterNetworkV6);

            byte[] Res = new byte[0];

            try
            {
                udp.Client.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, 0);
                System.Net.IPEndPoint localEP =
                    new System.Net.IPEndPoint(System.Net.IPAddress.IPv6Any, port);
                udp.Client.Bind(localEP);

                System.Net.IPEndPoint tmp = null;
                Res = udp.Receive(ref tmp);
            }
            finally
            {
                udp.Close();
            }

            return Res;
        }
    }
}