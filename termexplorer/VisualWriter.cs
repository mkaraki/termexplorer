using Get_Unicode_EastAsianWidth;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;
using static termexplorer.Config.ColorMap;
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

            ForegroundColor = DefaultTextColor;
            BackgroundColor = DefaultBackgroundColor;

            if (Config.WriteProductName)
            {
                WritedLine++;
                string titlepad = new string(' ', (WritableWidth - ProductInfo.Name.Length) / 2);
                Write(titlepad + ProductInfo.Name + titlepad + Environment.NewLine);
            }

            // Help Information
            WritedLine++;
            WriteLine("Press F1 to open help");

            // Write Files
            WritedLine += 2;
            Write(ContentSplit);

            int WritableFname = WritableWidth / 2 - 1 - 2;
            int FnameHeight = WritableHeight - 1 - WritedLine;
            string brankfn = new string(' ', WritableFname + 2);
            for (int i = 0; i < FnameHeight; i++)
            {
                // Start
                Write('|');

                //Window1
                WriteDirEntryWithLine(1, i, FnameHeight, WritableFname + 2);

                //Split
                ForegroundColor = DefaultTextColor;
                BackgroundColor = DefaultBackgroundColor;
                Write('|');

                //Window2
                WriteDirEntryWithLine(2, i, FnameHeight, WritableFname + 2);

                // End
                ForegroundColor = DefaultTextColor;
                BackgroundColor = DefaultBackgroundColor;
                Write('|');

                Write(Environment.NewLine);
            }

            Write(ContentSplit);
        }

        public static void WriteDirEntryWithLine(int WindowId, int CurrentLine, int WritableHeight, int WritableWidth)
        {
            int id = WindowId;

            if (ToWrite.CurrentWindow == id)
            {
                BackgroundColor = ContentBackgroundColor;
                ForegroundColor = EntryTextColor;
            }

            int w2point = CurrentLine;
            if (ToWrite.Windows[id].Files.Count > WritableHeight)
                w2point += ToWrite.Windows[id].CurrentPointer;

            if (w2point < ToWrite.Windows[id].Files.Count)
            {
                if (w2point == ToWrite.Windows[id].CurrentPointer)
                    Write("> ");
                else
                    Write("  ");

                if (ToWrite.Windows[id].Files[w2point].FileName != ".." && ToWrite.Windows[id].Files[w2point].IsDirectory)
                    ForegroundColor = DirectoryTextColor;
                Write(WriteEntryName(ToWrite.Windows[id].Files[w2point].FileName, WritableWidth - 2));
            }
            else
                Write(new string(' ', WritableWidth));
        }

        public static string WriteEntryName(string Original, int Writable)
        {
            int OriginalLength = EAWCheck.GetStrLenWithEAW(Original, true);
            int Pad = Writable - OriginalLength;
            if (Pad > 0)
                return Original + new string(' ', Pad);
            else
            {
                string toret = "";
                int ToWrite = Writable - 5;
                int CurrentLen = 0;
                foreach (char f_char in Original)
                {
                    toret += f_char;
                    bool ctype = EAWCheck.IsFullWidth(f_char, true);
                    if (ctype) CurrentLen += 2;
                    else CurrentLen += 1;
                    if (CurrentLen >= 5) break;
                }
                return toret;
            }
        }

        public static WriteInfo ToWrite = new WriteInfo();

        public class WriteInfo
        {
            public List<Window> Windows = new List<Window>()
            {
                null,
                new Window(Environment.CurrentDirectory),
                new Window(Environment.CurrentDirectory)
            };

            public int CurrentWindow = 1;

            public class Window
            {
                public Window(string dirPath, bool CantAccessError = false)
                {
                    Files = new List<FileInfo>();
                    try {
                        Files.Add(new FileInfo(dirPath, true));
                        HasParent = true;
                    }
                    catch { }

                    List<string> files = new List<string>();

                    files.AddRange(System.IO.Directory.GetDirectories(dirPath, "*", System.IO.SearchOption.TopDirectoryOnly).ToList());
                    files.AddRange(System.IO.Directory.GetFiles(dirPath, "*", System.IO.SearchOption.TopDirectoryOnly).ToList());

                    Current = new FileInfo(dirPath);

                    foreach (string fname in files)
                        Files.Add(new FileInfo(fname));
                }

                public bool HasParent;
                public FileInfo Current;
                public bool CantAccess = false;
                public List<FileInfo> Files;
                public int CurrentPointer = 0;
            }
        }

        public static void ChangeAddressWindow()
        {
            string OldPath;
            if (ToWrite.Windows[ToWrite.CurrentWindow].Files.Count < 1)
                OldPath = null;
            else
                OldPath = ToWrite.Windows[ToWrite.CurrentWindow].Current.FullPath;

            #region Writer

            string WindowName = $"Window {ToWrite.CurrentWindow}";

            Clear();
            int This_top = (WritableHeight / 2) - 3;

            if (Config.WriteProductName)
            {
                string titlepad = new string(' ', (WritableWidth - ProductInfo.Name.Length) / 2);
                WriteLine(titlepad + ProductInfo.Name + titlepad);
            }

            SetCursorPosition(0, This_top);
            string TargNamepad = new string(' ', (WritableWidth - WindowName.Length) / 2);
            WriteLine(TargNamepad + WindowName + TargNamepad);

            WriteLine();

            // Aveable Drive Show
            Write("Aveable Drives:");
            string[] drives = System.IO.Directory.GetLogicalDrives();
            foreach (string drv in drives)
                Write($" [{drv}]");
            WriteLine();
            WriteLine();

            WriteLine($"Current: {OldPath}");

            BackgroundColor = ConsoleColor.White;
            ForegroundColor = ConsoleColor.Black;

            Write(new string(' ', WritableWidth));
            SetCursorPosition(0, CursorTop);

            #endregion Writer

            string UCPath = ReadLine().Replace("\"", "");

            UserControl.ChangeDir(UCPath, OldPath ?? Environment.CurrentDirectory);

            ForegroundColor = DefaultTextColor;
            BackgroundColor = DefaultBackgroundColor;
        }
    }
}