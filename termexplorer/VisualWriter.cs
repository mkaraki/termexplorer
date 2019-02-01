﻿using Get_Unicode_EastAsianWidth;
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
            UIWriter.UIInfo ui = new UIWriter.UIInfo();

            //Clear();
            int WritedLine = 0;

            string ContentSplit = new string('-', WritableWidth);

            ui.ForegroundColor = "DefaultTextColor";
            ui.BackgroundColor = "DefaultBackgroundColor";

            if (Config.WriteProductName)
            {
                WritedLine++;
                string titlepad = new string(' ', (WritableWidth - ProductInfo.Name.Length) / 2);
                ui.WriteLine(titlepad + ProductInfo.Name + titlepad);
            }

            // Help Information
            WritedLine++;
            ui.WriteLine("Press F1 to open help");

            // Write Files
            WritedLine += 2;
            ui.WriteLine(ContentSplit);

            int WritableFname = WritableWidth / 2 - 1 - 2;
            int FnameHeight = WritableHeight - 1 - WritedLine;
            string brankfn = new string(' ', WritableFname + 2);
            for (int i = 0; i < FnameHeight; i++)
            {
                // Start
                ui.Write('|');

                //Window1
                ui.Write(WriteDirEntryWithLine(1, i, FnameHeight, WritableFname + 2));

                //Split
                ui.ForegroundColor = "DefaultTextColor";
                ui.BackgroundColor = "DefaultBackgroundColor";
                ui.Write('|');

                //Window2
                ui.Write(WriteDirEntryWithLine(2, i, FnameHeight, WritableFname + 2));

                // End
                ui.ForegroundColor = "DefaultTextColor";
                ui.BackgroundColor = "DefaultBackgroundColor";
                ui.Write('|');

                ui.WriteLine();
            }

            ui.WriteLine(ContentSplit);

            ui.WriteDown();
        }

        public static string WriteDirEntryWithLine(int WindowId, int CurrentLine, int WritableHeight, int WritableWidth)
        {
            string toret = "";
            int id = WindowId;

            if (ToWrite.CurrentWindow == id)
            {
                toret += UIWriter.GetBGColorCmd("ContentBackgroundColor");
                toret += UIWriter.GetFGColorCmd("EntryTextColor");
            }

            int wpoint = CurrentLine;
            if (ToWrite.Windows[id].Files.Count > WritableHeight)
                wpoint += ToWrite.Windows[id].CurrentPointer;

            if (wpoint < ToWrite.Windows[id].Files.Count)
            {
                // Pointer
                if (wpoint == ToWrite.Windows[id].CurrentPointer)
                    toret += (">");
                else
                    toret += (" ");

                // Selected
                if (ToWrite.Windows[id].Selected.Contains(wpoint))
                    toret += ("*");
                else
                    toret += (" ");

                // FileName
                if (ToWrite.Windows[id].Files[wpoint].FileName != ".." && ToWrite.Windows[id].Files[wpoint].IsDirectory)
                    toret += UIWriter.GetFGColorCmd("DirectoryTextColor");
                toret += (WriteEntryName(ToWrite.Windows[id].Files[wpoint].FileName, WritableWidth - 2));
            }
            else
                toret += (new string(' ', WritableWidth));

            return toret;
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

                    Selected = new List<int>();
                }

                // Can go to parent directory
                public bool HasParent;

                // Current Dir
                public FileInfo Current;

                //public bool CantAccess = false;
                public List<FileInfo> Files;
                public int CurrentPointer = 0;
                public List<int> Selected;
            }
        }

        public static void ChangeAddressWindow()
        {
            UIWriter.ClearLast();

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