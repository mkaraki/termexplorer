using System;
using System.Collections.Generic;
using System.Text;
using P2P_File_Transfer_Library;

namespace termexplorer.FileTransfer
{
    class Downloader
    {
        private static byte[] Download(int port = 2222)
        {
            return P2P_File_Transfer_Library.Download.WaitByte(port);
        }

        public static void DownloadFile(FileInfo basedir)
        {
            // Comform
            bool check1 = BoxWriter.CheckScreen("Really Receive File?", $"This progress can not stop.", false, BoxWriter.InfoType.Information);
            if (check1 == false) return;

            BoxWriter.Splash("Waiting for Uploader");
            byte[] recvdata = Download(2222);

            string UserHash = BoxWriter.AskToUserScreen("File Transfer", "Accual Hash", BoxWriter.InfoType.Information);
            if (Shared.GetHash(recvdata) != UserHash)
            {
                BoxWriter.PopScreen("File broken", $"Received file is broken", BoxWriter.InfoType.Error);
                return;
            }

            string sfname = BoxWriter.AskToUserScreen("File Transfer", "Put FileName (can't overwrite it)", BoxWriter.InfoType.Information);
            string fname = System.IO.Path.Combine(basedir.FullPath,sfname);
            if (System.IO.File.Exists(fname))
            {
                BoxWriter.PopScreen("FileName already used", $"Filename already used", BoxWriter.InfoType.Error);
                return;
            }

            // Comform
            bool check2 = BoxWriter.CheckScreen("Really Receive This?", $"The file will save to \"{fname}\"", false, BoxWriter.InfoType.Information);
            if (check2 == false) return;

            System.IO.FileStream fs = new System.IO.FileStream(fname,System.IO.FileMode.Create,System.IO.FileAccess.Write);
            fs.Write(recvdata, 0, recvdata.Length);
            fs.Close();

            BoxWriter.PopScreen("Success", $"The file was saved to \"{fname}\"", BoxWriter.InfoType.Success);
        }
    }
}
