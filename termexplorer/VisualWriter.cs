using Get_Unicode_EastAsianWidth;
using System;
using System.Collections.Generic;
using System.Linq;
using static termexplorer.Config.ColorMap;
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

            ForegroundColor = DefaultTextColor;
            Console.BackgroundColor = Config.ColorMap.BackgroundColor;

            if (Config.WriteProductName)
            {
                WritedLine++;
                string titlepad = new string(' ', (WritableWidth - ProductInfo.Name.Length) / 2);
                Write(titlepad + ProductInfo.Name + titlepad + Environment.NewLine);
            }

            //TODO: Write Options
            WritedLine++;
            WriteLine("Alt+D: Change Dir");

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
                Console.BackgroundColor = Config.ColorMap.BackgroundColor;
                Write('|');

                //Window2
                WriteDirEntryWithLine(2, i, FnameHeight, WritableFname + 2);

                // End
                ForegroundColor = DefaultTextColor;
                Console.BackgroundColor = Config.ColorMap.BackgroundColor;
                Write('|');

                Write(Environment.NewLine);
            }

            Write(ContentSplit);
        }

        public static void WriteDirEntryWithLine(int WindowId,int CurrentLine,int WritableHeight,int WritableWidth)
        {
            int id = WindowId;

            if (ToWrite.CurrentWindow == id)
            {
                Console.BackgroundColor = ContentBackgroundColor;
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
                Write(new string(' ',WritableWidth));
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
                    try { Files.Add(new FileInfo(dirPath, true)); }
                    catch { }

                    List<string> files = new List<string>();

                    files.AddRange(System.IO.Directory.GetDirectories(dirPath, "*", System.IO.SearchOption.TopDirectoryOnly).ToList());
                    files.AddRange(System.IO.Directory.GetFiles(dirPath, "*", System.IO.SearchOption.TopDirectoryOnly).ToList());

                    Current = new FileInfo(dirPath);

                    foreach (string fname in files)
                        Files.Add(new FileInfo(fname));
                }

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

            Console.BackgroundColor = ConsoleColor.White;
            ForegroundColor = ConsoleColor.Black;

            Write(new string(' ', WritableWidth));
            SetCursorPosition(0, CursorTop);

            #endregion

            string UCPath = ReadLine().Replace("\"", "");

            ChangeDir(UCPath, OldPath ?? Environment.CurrentDirectory);

            ForegroundColor = DefaultTextColor;
            Console.BackgroundColor = Config.ColorMap.BackgroundColor;
        }

        public static void ChangeDir(string Path, string OldPath)
        {
            try
            {
                System.IO.Directory.GetDirectories(Path);
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                ErrorScreen("No Directory", $"Selected path is not found");
                Path = OldPath;
            }
            catch (UnauthorizedAccessException)
            {
                ErrorScreen("Access Denied", $"You don't have permission to access selected directory");
                Path = OldPath;
            }
            catch (System.IO.IOException)
            {
                ErrorScreen("Not Directory", $"Selected path is not directory");
                Path = OldPath;
            }
            catch (ArgumentException)
            {
                Path = OldPath;
            }

            ToWrite.Windows[ToWrite.CurrentWindow] = new WriteInfo.Window(Path);
        }

        public static void ErrorScreen(string ErrorTitle, string ErrorDetails)
        {
            Console.BackgroundColor = ErrorBackgroundColor;
            ForegroundColor = ErrorTextColor;

            Clear();
            int This_top = (WritableHeight / 2) - 4;

            if (Config.WriteProductName)
            {
                string titlepad = new string(' ', (WritableWidth - ProductInfo.Name.Length) / 2);
                Write(titlepad + ProductInfo.Name + titlepad + Environment.NewLine);
            }

            SetCursorPosition(0, This_top);
            string ErrTitlepad = new string(' ', (WritableWidth - ErrorTitle.Length) / 2);
            Write(ErrTitlepad + ErrorTitle + ErrTitlepad + Environment.NewLine);

            SetCursorPosition(0, This_top + 2);
            WriteLine(ErrorDetails);

            WriteLine(Environment.NewLine);
            WriteLine("Press Enter to continue.");
            ReadLine();
        }
    }
}