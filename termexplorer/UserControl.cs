using System;
using System.Collections.Generic;
using static termexplorer.VisualWriter;

namespace termexplorer
{
    internal class UserControl
    {
        public static void Handle(ConsoleKeyInfo cki)
        {
            int cwin = ToWrite.CurrentWindow;

            // Change Address
            if (cki.Modifiers == ConsoleModifiers.Alt && cki.Key == ConsoleKey.D) ChangeAddressWindow();
            // Select Content
            #region
            else if (cki.Key == ConsoleKey.UpArrow)
            {
                if (ToWrite.Windows[cwin].CurrentPointer > 0)
                    ToWrite.Windows[cwin].CurrentPointer--;
            }
            else if (cki.Key == ConsoleKey.DownArrow)
            {
                if (ToWrite.Windows[cwin].CurrentPointer < ToWrite.Windows[cwin].Files.Count - 1)
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
            else if (cki.Key == ConsoleKey.LeftArrow) ChangeWindow(true);
            else if (cki.Key == ConsoleKey.RightArrow) ChangeWindow(false);
            #endregion
            // Help
            else if (cki.Key == ConsoleKey.F1) DocumentWriter.VWriter.LoadDocumentWriter(DocumentWriter.InternalDocuments.Help.Content);
            // Update
            else if (cki.Key == ConsoleKey.F5) Update(cwin);
            // Delete
            else if (cki.Key == ConsoleKey.Delete) DeleteFile(cwin);
            // Back to parent
            else if (cki.Key == ConsoleKey.Backspace) GoParent(cwin);
            // Select/Unselect
            else if (cki.Key == ConsoleKey.Spacebar) ToggleSelect(cwin);
            // Rename
            else if (cki.Key == ConsoleKey.F2) Rename(cwin);
            // Send File
            else if (cki.Key == ConsoleKey.S) FileTransfer.Sender.SendFile(ToWrite.Windows[cwin].Files[ToWrite.Windows[cwin].CurrentPointer]);
            // Receive File
            else if (cki.Key == ConsoleKey.R) FileTransfer.Downloader.DownloadFile(ToWrite.Windows[cwin].Current);
            // Search
            else if (cki.Key == ConsoleKey.F3) SearchWithName(cwin);
            // Windows Add/Rem
            else if (cki.Key == ConsoleKey.OemPlus||cki.Key == ConsoleKey.Add) AddWindow();
            else if (cki.Key == ConsoleKey.OemMinus || cki.Key == ConsoleKey.Subtract) RemoveWindow(cwin);
        }

        public static void AddWindow()
        {
            ToWrite.Windows.Add(new WriteInfo.Window(Environment.CurrentDirectory));
        }

        public static void RemoveWindow(int win)
        {
            if (ToWrite.Windows.Count <= 2) return;
            if (ToWrite.Windows.Count - 1 == win)
                ToWrite.CurrentWindow = win - 1;
            ToWrite.Windows.RemoveAt(win);
        }

        public static void SearchWithName(int win)
        {
            string pattern = BoxWriter.AskToUserScreen("Search by Name", $"Search string\nBrank to abort.", BoxWriter.InfoType.Information);
            if (pattern == "") return;

            bool sf = BoxWriter.CheckScreen("Search subfolder", $"Search subfolder too?" ,true, BoxWriter.InfoType.Information);

            BoxWriter.Splash("Searching");

            List<FileInfo> files = FileSearch.Search.SearchWithName(ToWrite.Windows[win].Current, pattern, sf);

            files.Insert(0, new FileInfo(ToWrite.Windows[win].Current.FullPath)
            {
                FileName = "Back"
            });

            ToWrite.Windows[win].CurrentPointer = 0;
            ToWrite.Windows[win].Selected = new List<int>();
            ToWrite.Windows[win].Files = files;
        }

        public static void Rename(int win)
        {
            FileInfo finfo = ToWrite.Windows[win].Files[ToWrite.Windows[win].CurrentPointer];

            string dest_name = BoxWriter.AskToUserScreen("Rename",$"Type new name\n\nOldName: {finfo.FileName}",BoxWriter.InfoType.Information);
            if (dest_name == "") return;
            string path = System.IO.Path.Combine(ToWrite.Windows[win].Current.FullPath,dest_name);

            if (finfo.IsDirectory)
            {
                System.IO.Directory.Move(finfo.FullPath, path);
            }
            else
            {
                System.IO.File.Move(finfo.FullPath, path);
            }

            ChangeDirInternal(ToWrite.Windows[win].Current.FullPath);
        }

        public static void ToggleSelect(int win)
        {
            if (ToWrite.Windows[win].Selected.Contains(ToWrite.Windows[win].CurrentPointer))
                // Remove from selected
                ToWrite.Windows[win].Selected.Remove(ToWrite.Windows[win].CurrentPointer);
            else
                // Add to selected
                ToWrite.Windows[win].Selected.Add(ToWrite.Windows[win].CurrentPointer);
        }

        public static void GoParent(int win)
        {
            if (!ToWrite.Windows[win].HasParent) return;
            ChangeDir(ToWrite.Windows[win].Files[0].FullPath, ToWrite.Windows[win].Current.FullPath);
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
            UIWriter.ClearLast();
            System.Diagnostics.Process.Start(Config.Program_Exec_Path, $"\"{FilePath}\"");
        }

        public static void DeleteFile(int win)
        {
            string fname = ToWrite.Windows[win].Files[ToWrite.Windows[win].CurrentPointer].FullPath;

            if (ToWrite.Windows[win].Files[ToWrite.Windows[win].CurrentPointer].IsDirectory)
            {
                bool check = BoxWriter.CheckScreen("Really Delete this directory?", $"\"{fname}\"\nWill be deleted abd cannot undo it.", false, BoxWriter.InfoType.Error);
                if (check == false) return;

                System.IO.Directory.Delete(fname);
            }
            else
            {
                bool check = BoxWriter.CheckScreen("Really Delete this?", $"\"{fname}\"\nWill be deleted and cannot undo.", false, BoxWriter.InfoType.Error);
                if (check == false) return;

                System.IO.File.Delete(fname);
            }

            ChangeDirInternal(ToWrite.Windows[win].Current.FullPath);
        }

        public static void Update(int win)
        {
            ChangeDirInternal(ToWrite.Windows[win].Current.FullPath);
        }

        public static void ChangeDirInternal(string Path)
        {
            ToWrite.Windows[ToWrite.CurrentWindow] = new WriteInfo.Window(Path);
        }

        public static void ChangeDir(string Path, string OldPath)
        {
            try
            {
                System.IO.Directory.GetDirectories(Path);
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                BoxWriter.PopScreen("No Directory", $"Selected path is not found", BoxWriter.InfoType.Error);
                Path = OldPath;
            }
            catch (UnauthorizedAccessException)
            {
                BoxWriter.PopScreen("Access Denied", $"You don't have permission to access selected directory", BoxWriter.InfoType.Error);
                Path = OldPath;
            }
            catch (System.IO.IOException)
            {
                BoxWriter.PopScreen("Not Directory", $"Selected path is not directory", BoxWriter.InfoType.Error);
                Path = OldPath;
            }
            catch (ArgumentException)
            {
                Path = OldPath;
            }

            ToWrite.Windows[ToWrite.CurrentWindow] = new WriteInfo.Window(Path);
        }
    }
}