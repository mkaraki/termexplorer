using System;
using static termexplorer.VisualWriter;

namespace termexplorer
{
    internal class UserControl
    {
        public static void Handle(ConsoleKeyInfo cki)
        {
            int cwin = ToWrite.CurrentWindow;

            // Select Content
            #region
            if (cki.Key == ConsoleKey.UpArrow)
            {
                if (ToWrite.Windows[cwin].CurrentPointer > 0)
                    ToWrite.Windows[cwin].CurrentPointer--;
            }
            else if (cki.Key == ConsoleKey.DownArrow)
            {
                if (ToWrite.Windows[cwin].CurrentPointer < ToWrite.Windows[1].Files.Count - 1)
                    ToWrite.Windows[cwin].CurrentPointer++;
            }
            #endregion
            // Press Enter
            #region
            else if (cki.Key == ConsoleKey.Enter)
                if (ToWrite.Windows[cwin].Files[ToWrite.Windows[cwin].CurrentPointer].IsDirectory)
                    ChangeDir(ToWrite.Windows[cwin].Files[ToWrite.Windows[cwin].CurrentPointer].FullPath,
                        ToWrite.Windows[cwin].Current.FullPath);
                else
                    OpenFile(ToWrite.Windows[cwin].Files[ToWrite.Windows[cwin].CurrentPointer].FullPath);
            #endregion
            // Change Window
            #region
            else if (cki.Key == ConsoleKey.LeftArrow)
            {
                ChangeWindow(true);
            }
            else if (cki.Key == ConsoleKey.RightArrow)
            {
                ChangeWindow(false);
            }
            #endregion
            // Change Address
            else if (cki.Modifiers == ConsoleModifiers.Alt && cki.Key == ConsoleKey.D) ChangeAddressWindow();
        }

        public static void ChangeWindow(bool LeftDirection)
        {
            if (LeftDirection)
                if (ToWrite.CurrentWindow > 1)
                    ToWrite.CurrentWindow--;
                else return;
            else
                if (ToWrite.CurrentWindow < ToWrite.Windows.Count - 1)
                ToWrite.CurrentWindow++;
            else return;
        }

        public static void OpenFile(string FilePath)
        {
            Console.Clear();
            System.Diagnostics.Process.Start(Config.Program_Exec_Path, $"\"{FilePath}\"");
        }
    }
}