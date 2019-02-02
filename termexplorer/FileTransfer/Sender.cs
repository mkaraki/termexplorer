using System;
using System.Collections.Generic;
using System.Text;
using P2P_File_Transfer_Library;

namespace termexplorer.FileTransfer
{
    class Sender
    {
        public static void SendFile(string file, string ipaddress, int port = 2222)
        {
            Upload.Send(file,ipaddress,port);

            GC.Collect();
        }
    }
}
