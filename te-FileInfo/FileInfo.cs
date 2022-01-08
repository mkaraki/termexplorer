using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace termexplorer
{
    public class FileInfo
    {
        public FileInfo(string FilePath,bool SetParent = false)
        {
            DirectoryInfo Parentfinfo = Directory.GetParent(FilePath);
            if (SetParent && Parentfinfo == null)
            {
                // For Root directory (..)
                FileName = ". (Root)";
                FileExtension = "";
                FileSize = 0;
                IsDirectory = true;

                FullPath = FilePath;
                CreatedDate = DateTime.Now;
                UpdatedDate = DateTime.Now;

                return;
            }
            else if (SetParent)
            {
                // Generate ..
                FileName = "..";
                FileExtension = "";
                FileSize = 0;
                IsDirectory = true;

                FullPath = Parentfinfo.FullName;
                CreatedDate = Parentfinfo.CreationTime;
                UpdatedDate = Parentfinfo.LastWriteTime;

                return;
            }

            if (!File.Exists(FilePath) && !Directory.Exists(FilePath))
                throw new FileNotFoundException($"\"{FilePath}\" not found");

            System.IO.FileInfo finfo = new System.IO.FileInfo(FilePath);

            finfo.Refresh();

            if (!File.Exists(FilePath) && Directory.Exists(FilePath))
            {
                IsDirectory = true;
                FileSize = 0;
                FileExtension = "";
            }
            else
            {
                FileSize = finfo.Length;
                FileExtension = finfo.Extension;
            }

            FullPath = finfo.FullName;
            FileName = finfo.Name;
            CreatedDate = finfo.CreationTime;
            UpdatedDate = finfo.LastWriteTime;
        }

        public string FullPath;
        public string FileName;
        public string FileExtension;
        public bool IsDirectory;

        public DateTime CreatedDate;
        public DateTime UpdatedDate;

        public long FileSize;
    }
}
