using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;
using static termexplorer.ConsoleInfo;

namespace termexplorer
{
    internal class VisualWriter
    {
        public static void SWrite()
        {
            Clear();
            int WritedLine = 0;

            string ContentSplit = new string('-', WritableWidth) + Environment.NewLine;

            ForegroundColor = ConsoleColor.White;
            BackgroundColor = ConsoleColor.Blue;

            if (Config.WriteProductName)
            {
                WritedLine++;
                string titlepad = new string(' ', (WritableWidth - ProductInfo.Name.Length) / 2);
                Write(titlepad + ProductInfo.Name + titlepad + Environment.NewLine);
            }

            //TODO: Write Options
            WritedLine++;
            Write(Environment.NewLine);

            // Write Files
            WritedLine += 2;
            Write(ContentSplit);

            int WritableFname = WritableWidth / 2 -1 - 2;
            int FnameHeight = WritableHeight - 1 - WritedLine;
            string brankfn = new string(' ', WritableFname + 2);
            for (int i = 0; i < FnameHeight; i++)
            {
                // Start
                Write('|');

                //Window1
                #region
                if (ToWrite.CurrentWindow == 1)
                {
                    if (ToWrite.Window1.CantAccess)
                        BackgroundColor = ConsoleColor.DarkGray;
                    else
                        BackgroundColor = ConsoleColor.White;
                    ForegroundColor = ConsoleColor.Black;
                }

                int w1point = i;
                if (ToWrite.Window1.Files.Count > FnameHeight)
                    w1point += ToWrite.Window1.CurrentPointer;

                if (w1point < ToWrite.Window1.Files.Count)
                {
                    if (w1point == ToWrite.Window1.CurrentPointer)
                        Write("> ");
                    else
                        Write("  ");
                    if (ToWrite.Window1.Files[w1point].FileName != ".." && ToWrite.Window1.Files[w1point].IsDirectory)
                        ForegroundColor = ConsoleColor.Green;
                    Write(ToWrite.Window1.Files[w1point].FileName.PadRight(WritableFname));
                }
                else
                    Write(brankfn);
                #endregion

                //Split
                ForegroundColor = ConsoleColor.White;
                BackgroundColor = ConsoleColor.Blue;
                Write('|');

                //Window2
                #region
                if (ToWrite.CurrentWindow == 2)
                {
                    if (ToWrite.Window2.CantAccess)
                        BackgroundColor = ConsoleColor.DarkGray;
                    else
                        BackgroundColor = ConsoleColor.White;
                    ForegroundColor = ConsoleColor.Black;
                }

                int w2point = i;
                if (ToWrite.Window2.Files.Count > FnameHeight)
                    w2point += ToWrite.Window2.CurrentPointer;

                if (w2point < ToWrite.Window2.Files.Count)
                {
                    if (w2point == ToWrite.Window2.CurrentPointer)
                        Write("> ");
                    else
                        Write("  ");

                    if (ToWrite.Window2.Files[w2point].FileName != ".." && ToWrite.Window2.Files[w2point].IsDirectory)
                        ForegroundColor = ConsoleColor.Green;
                    Write(ToWrite.Window2.Files[w2point].FileName.PadRight(WritableFname));
                }
                else
                    Write(brankfn);
                #endregion

                // End
                ForegroundColor = ConsoleColor.White;
                BackgroundColor = ConsoleColor.Blue;
                Write('|');

                Write(Environment.NewLine);
            }

            Write(ContentSplit);
        }

        public static WriteInfo ToWrite = new WriteInfo();

        public class WriteInfo
        {
            public Window Window1 = new Window(Environment.CurrentDirectory);
            public Window Window2 = new Window(@"C:\");

            public int CurrentWindow = 1;

            public class Window
            {
                public Window(string dirPath)
                {
                    Files = new List<FileInfo>();
                    try { Files.Add(new FileInfo(dirPath, true)); }
                    catch { }

                    List<string> files = new List<string>();
                    try
                    {
                        files.AddRange(System.IO.Directory.GetDirectories(dirPath, "*", System.IO.SearchOption.TopDirectoryOnly).ToList());
                    }
                    catch (UnauthorizedAccessException)
                    {
                        CantAccess = true;
                        return;
                    }
                    files.AddRange(System.IO.Directory.GetFiles(dirPath, "*", System.IO.SearchOption.TopDirectoryOnly).ToList());

                    foreach (string fname in files)
                        Files.Add(new FileInfo(fname));
                }

                public bool CantAccess = false;
                public List<FileInfo> Files;
                public int CurrentPointer = 0;
            }
        }

        public static void ChangeAddressWindow()
        {
            if (ToWrite.CurrentWindow == 1)
            {
                ChangeAddressWindowWriter("Window1",ToWrite.Window1.Files[0].FullPath);
                string UCPath = ReadLine();
                ToWrite.Window1 = new WriteInfo.Window(UCPath);
            }
            else if (ToWrite.CurrentWindow == 2)
            {
                ChangeAddressWindowWriter("Window1",ToWrite.Window1.Files[0].FullPath);
                string UCPath = ReadLine();
                ToWrite.Window2 = new WriteInfo.Window(UCPath);
            }

            ForegroundColor = ConsoleColor.White;
            BackgroundColor = ConsoleColor.Blue;
        }
        private static void ChangeAddressWindowWriter(string TargetName,string DefaultLocation)
        {
            Clear();
            int This_top = (WritableHeight / 2) - 2;

            if (Config.WriteProductName)
            {
                string titlepad = new string(' ', (WritableWidth - ProductInfo.Name.Length) / 2);
                Write(titlepad + ProductInfo.Name + titlepad + Environment.NewLine);
            }

            SetCursorPosition(0,This_top);
            string TargNamepad = new string(' ', (WritableWidth - TargetName.Length) / 2);
            Write(TargNamepad + TargetName + TargNamepad + Environment.NewLine);
            BackgroundColor = ConsoleColor.White;
            ForegroundColor = ConsoleColor.Black;
            Write(new string(' ', WritableWidth));
            SetCursorPosition(0,This_top+1);
        }

    }
}