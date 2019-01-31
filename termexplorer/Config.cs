using System;
using Newtonsoft.Json;

namespace termexplorer
{
    internal class ConfigManager
    {
        public static void ReadConfig()
        {

        }
    }

    internal class Config
    {
        public static bool WriteProductName { get; set; }
        public static string Program_Exec_Path { get; set; }

        internal class ColorMap
        {
            public static ConsoleColor DefaultTextColor { get; set; }
            public static ConsoleColor EntryTextColor { get; set; }
            public static ConsoleColor DirectoryTextColor { get; set; }
            public static ConsoleColor ContentBackgroundColor { get; set; }
            public static ConsoleColor BackgroundColor { get; set; }
            public static ConsoleColor ErrorBackgroundColor { get; set; }
            public static ConsoleColor ErrorTextColor { get; set; }
        }
    }

    internal class DefaultConfig
    {
        public static bool WriteProductName = true;
        public static string Program_Exec_Path = @"D:\Projects\termexplorer\ExecWindows\bin\Debug\ExecWindows.exe";

        internal class ColorMap
        {
            public static ConsoleColor DefaultTextColor = UsableColors.White;
            public static ConsoleColor EntryTextColor = UsableColors.Black;
            public static ConsoleColor DirectoryTextColor = UsableColors.Green;
            public static ConsoleColor ContentBackgroundColor = UsableColors.White;
            public static ConsoleColor BackgroundColor = UsableColors.Blue;
            public static ConsoleColor ErrorBackgroundColor = UsableColors.DarkRed;
            public static ConsoleColor ErrorTextColor = UsableColors.White;
        }
    }

    public class UsableColors
    {
        public static ConsoleColor Black = ConsoleColor.Black;
        public static ConsoleColor DarkBlue = ConsoleColor.DarkBlue;
        public static ConsoleColor DarkCyan = ConsoleColor.DarkCyan;
        public static ConsoleColor DarkRed = ConsoleColor.DarkRed;
        public static ConsoleColor DarkMagenta = ConsoleColor.DarkMagenta;
        public static ConsoleColor DarkYellow = ConsoleColor.DarkYellow;
        public static ConsoleColor Gray = ConsoleColor.Gray;
        public static ConsoleColor DarkGray = ConsoleColor.DarkGray;
        public static ConsoleColor Blue = ConsoleColor.Blue;
        public static ConsoleColor Green = ConsoleColor.Green;
        public static ConsoleColor Cyan = ConsoleColor.Cyan;
        public static ConsoleColor Red = ConsoleColor.Red;
        public static ConsoleColor Magenta = ConsoleColor.Magenta;
        public static ConsoleColor Yellow = ConsoleColor.Yellow;
        public static ConsoleColor White = ConsoleColor.White;
    }
}