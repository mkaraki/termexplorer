﻿using System;
using System.Collections.Generic;
using System.Text;
using P2P_File_Transfer_Library;

namespace termexplorer.FileTransfer
{
    class Sender
    {
        public static void SendFile(FileInfo finfo)
        {
            string fname = finfo.FullPath;

            if (!System.IO.File.Exists(fname))
            {
                BoxWriter.PopScreen("Not file", "This feature is only support for file.", BoxWriter.InfoType.Error);
                return;
            }

            if (finfo.FileSize > 60328)
            {
                BoxWriter.PopScreen("File size is too large!", "This feature is only support less than 60,328 bytes file", BoxWriter.InfoType.Error);
                return;
            }

            // Comform
            bool check = BoxWriter.CheckScreen("Really Send this file?", $"File :\"{fname}\"", false, BoxWriter.InfoType.Information);
            if (check == false) return;

            // IP Address
            string ip = BoxWriter.AskToUserScreen("File Transfer", "Target IP Address", BoxWriter.InfoType.Information);
            System.Net.IPAddress ipa;
            if (!System.Net.IPAddress.TryParse(ip, out ipa))
            {
                BoxWriter.PopScreen("Not valid IP Address", $"\"{ip}\" is not valid IP Address", BoxWriter.InfoType.Error);
                return;
            }

            // Calc Hash
            string Hash = Shared.GetFileHash(fname);

            SendFile(fname, ip, 2222);

            BoxWriter.PopScreen("Success", $"Progless were successful.\nPlease note this hash and tell hash to user.\nHash: {Hash}", BoxWriter.InfoType.Success);
        }

        private static void SendFile(string file, string ipaddress, int port = 2222)
        {
            Upload.Send(file, ipaddress, port);

            GC.Collect();
        }
    }
}
