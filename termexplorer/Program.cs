using System;

namespace termexplorer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ConfigManager.LoadDefault();
            ConfigManager.ReadConfig();

            ConsoleInfo.UpdateConsoleInfo();
            if (!System.IO.File.Exists(Config.Program_Exec_Path))
            {
                VisualWriter.ErrorScreen("File Missing!", $"Important file is missing\nFile: termexplorer - Exec (\"{Config.Program_Exec_Path}\")");
            }

            while (true)
            {
                ConsoleInfo.UpdateConsoleInfo();
                VisualWriter.SWrite();
                ConsoleKeyInfo cki = Console.ReadKey(false);
                if (cki.KeyChar == 'q')
                    break;
                UserControl.Handle(cki);
            }
        }
    }
}