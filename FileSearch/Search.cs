using System;
using termexplorer;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileSearch
{
    public class Search
    {
        public static List<termexplorer.FileInfo> SearchWithName(termexplorer.FileInfo Target,string Pattern,bool Subfolder)
        {
            List<termexplorer.FileInfo> toret = new List<termexplorer.FileInfo>();
            List<string> files;

            if (Subfolder)
            {
                files = GetAllFiles(Target.FullPath,Pattern);
            }
            else
            {
                files = Directory.GetFiles(Target.FullPath, $"*{Pattern}*",SearchOption.TopDirectoryOnly).ToList();
            }

            foreach (string fn in files)
                toret.Add(new termexplorer.FileInfo(fn));

            return toret;
        }

        public static List<string> GetAllFiles(string path,string pattern)
        {
            List<string> toret = new List<string>();

            try
            {
                string[] files = Directory.GetFiles(path,$"*{pattern}*");
                foreach (string fname in files)
                {
                    toret.Add(fname);
                }

                string[] dirs = Directory.GetDirectories(path);
                foreach (string dname in dirs)
                {
                    List<string> fs = GetAllFiles(dname,pattern);
                    foreach (string fname in fs)
                        toret.Add(fname);
                }
            }
            catch (UnauthorizedAccessException) { }

            return toret;
        }

    }
}
