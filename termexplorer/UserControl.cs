using System;
using static termexplorer.VisualWriter;

namespace termexplorer
{
    internal class UserControl
    {
        public static void Handle(ConsoleKeyInfo cki)
        {
            // Select Content
            #region
            if (cki.Key == ConsoleKey.UpArrow)
            {
                if (ToWrite.CurrentWindow == 1)
                    if (ToWrite.Window1.CurrentPointer > 0)
                        ToWrite.Window1.CurrentPointer--;
                if (ToWrite.CurrentWindow == 2)
                    if (ToWrite.Window2.CurrentPointer > 0)
                        ToWrite.Window2.CurrentPointer--;
            }
            else if (cki.Key == ConsoleKey.DownArrow)
            {
                if (ToWrite.CurrentWindow == 1)
                    if (ToWrite.Window1.CurrentPointer < ToWrite.Window1.Files.Count - 1)
                        ToWrite.Window1.CurrentPointer++;
                if (ToWrite.CurrentWindow == 2)
                    if (ToWrite.Window2.CurrentPointer < ToWrite.Window2.Files.Count - 1)
                        ToWrite.Window2.CurrentPointer++;
            }
            #endregion
            // Press Enter
            else if (cki.Key == ConsoleKey.Enter)
            {
                if (ToWrite.CurrentWindow == 1)
                    if (ToWrite.Window1.Files[ToWrite.Window1.CurrentPointer].IsDirectory)
                        ToWrite.Window1 = new WriteInfo.Window(ToWrite.Window1.Files[ToWrite.Window1.CurrentPointer].FullPath);
                    else
                        OpenFile(ToWrite.Window1.Files[ToWrite.Window1.CurrentPointer].FullPath);
                if (ToWrite.CurrentWindow == 2)
                    if (ToWrite.Window2.Files[ToWrite.Window2.CurrentPointer].IsDirectory)
                        ToWrite.Window2 = new WriteInfo.Window(ToWrite.Window2.Files[ToWrite.Window2.CurrentPointer].FullPath);
                    else
                        OpenFile(ToWrite.Window2.Files[ToWrite.Window2.CurrentPointer].FullPath);
            }
            // Change Window
            #region
            else if (cki.Key == ConsoleKey.RightArrow)
            {
                if (ToWrite.CurrentWindow == 1)
                    ToWrite.CurrentWindow = 2;
            }
            else if (cki.Key == ConsoleKey.LeftArrow)
            {
                if (ToWrite.CurrentWindow == 2)
                    ToWrite.CurrentWindow = 1;
            }
            #endregion
        }

        public static void OpenFile(string FilePath)
        {
            System.Diagnostics.Process.Start(FilePath);
        }
    }
}