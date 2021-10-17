using System;
using System.Collections.Generic;

namespace termexplorer
{
    internal class ConfigManager
    {
        public static string ConfPath = "termexplorer.conf";

        public static void InitConfig()
        {
            LoadDefault();
            ReadConfig();

            UIWriter.SetColorCode();
        }

        private static void ReadConfig()
        {
            if (!System.IO.File.Exists(ConfPath))
            {
                Program.Logging(Program.LogLevel.Information, $"Config file not found in \"{ConfPath}\".");
                return;
            }

            string[] configs = System.IO.File.ReadAllLines(ConfPath);
            foreach (string confstr in configs)
            {
                string[] cnf = confstr.Split('|');
                if (cnf.Length != 2) continue;
                switch (cnf[0])
                {
                    case "WriteProductName":
                        Config.WriteProductName = StrBool(cnf[1]); break;
                    case "WriteEOFinReader":
                        Config.WriteEOFinReader = StrBool(cnf[1]); break;
                    case "Program_Exec_Path":
                        Config.Program_Exec_Path = cnf[1]; break;
                    case "ColorMap.DefaultTextColor":
                        Config.ColorMap.DefaultTextColor = UsableColors.strckey[cnf[1]]; break;
                    case "ColorMap.EntryTextColor":
                        Config.ColorMap.EntryTextColor = UsableColors.strckey[cnf[1]]; break;
                    case "ColorMap.DirectoryTextColor":
                        Config.ColorMap.DirectoryTextColor = UsableColors.strckey[cnf[1]]; break;
                    case "ColorMap.ContentBackgroundColor":
                        Config.ColorMap.ContentBackgroundColor = UsableColors.strckey[cnf[1]]; break;
                    case "ColorMap.DefaultBackgroundColor":
                        Config.ColorMap.DefaultBackgroundColor = UsableColors.strckey[cnf[1]]; break;
                    case "ColorMap.TextBoxBackgroundColor":
                        Config.ColorMap.TextBoxBackgroundColor = UsableColors.strckey[cnf[1]]; break;
                    case "ColorMap.TextBoxTextColor":
                        Config.ColorMap.TextBoxTextColor = UsableColors.strckey[cnf[1]]; break;
                    case "ColorMap.ErrorBackgroundColor":
                        Config.ColorMap.ErrorBackgroundColor = UsableColors.strckey[cnf[1]]; break;
                    case "ColorMap.ErrorTextColor":
                        Config.ColorMap.ErrorTextColor = UsableColors.strckey[cnf[1]]; break;
                    case "ColorMap.SuccessBackgroundColor":
                        Config.ColorMap.SuccessBackgroundColor = UsableColors.strckey[cnf[1]]; break;
                    case "ColorMap.SuccessTextColor":
                        Config.ColorMap.SuccessTextColor = UsableColors.strckey[cnf[1]]; break;
                }
            }

            Program.Logging(Program.LogLevel.Information, $"Log file (\"{ConfPath}\") loaded.");
        }

        private static void LoadDefault()
        {
            Config.WriteProductName = DefaultConfig.WriteProductName;
            Config.WriteEOFinReader = DefaultConfig.WriteEOFinReader;
            Config.Program_Exec_Path = DefaultConfig.Program_Exec_Path;

            Config.ColorMap.DefaultTextColor = DefaultConfig.ColorMap.DefaultTextColor;
            Config.ColorMap.EntryTextColor = DefaultConfig.ColorMap.EntryTextColor;
            Config.ColorMap.DirectoryTextColor = DefaultConfig.ColorMap.DirectoryTextColor;
            Config.ColorMap.ContentBackgroundColor = DefaultConfig.ColorMap.ContentBackgroundColor;
            Config.ColorMap.DefaultBackgroundColor = DefaultConfig.ColorMap.DefaultBackgroundColor;
            Config.ColorMap.TextBoxBackgroundColor = DefaultConfig.ColorMap.TextBoxBackgroundColor;
            Config.ColorMap.TextBoxTextColor = DefaultConfig.ColorMap.TextBoxTextColor;
            Config.ColorMap.ErrorBackgroundColor = DefaultConfig.ColorMap.ErrorBackgroundColor;
            Config.ColorMap.ErrorTextColor = DefaultConfig.ColorMap.ErrorTextColor;
            Config.ColorMap.SuccessBackgroundColor = DefaultConfig.ColorMap.SuccessBackgroundColor;
            Config.ColorMap.SuccessTextColor = DefaultConfig.ColorMap.SuccessTextColor;

            UIWriter.SetColorCode();
        }

        private static bool StrBool(string str)
        {
            switch (str)
            {
                case "true":
                    return true;

                case "false":
                    return false;

                default:
                    return false;
            }
        }
    }

    internal class Config
    {
        public static bool WriteProductName { get; set; }
        public static bool WriteEOFinReader { get; set; }
        public static string Program_Exec_Path { get; set; }

        internal class ColorMap
        {
            // Main
            public static ConsoleColor DefaultBackgroundColor { get; set; }

            // Explorer
            public static ConsoleColor DefaultTextColor { get; set; }

            public static ConsoleColor EntryTextColor { get; set; }
            public static ConsoleColor DirectoryTextColor { get; set; }
            public static ConsoleColor ContentBackgroundColor { get; set; }

            // Text Box
            public static ConsoleColor TextBoxBackgroundColor { get; set; }

            public static ConsoleColor TextBoxTextColor { get; set; }

            // Error
            public static ConsoleColor ErrorBackgroundColor { get; set; }

            public static ConsoleColor ErrorTextColor { get; set; }

            // Success
            public static ConsoleColor SuccessBackgroundColor { get; set; }

            public static ConsoleColor SuccessTextColor { get; set; }
        }
    }

    internal class DefaultConfig
    {
        public static bool WriteProductName = true;
        public static bool WriteEOFinReader = false;
        public static string Program_Exec_Path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ExecWindows.exe");

        internal class ColorMap
        {
            public static ConsoleColor DefaultTextColor = UsableColors.White;
            public static ConsoleColor EntryTextColor = UsableColors.Black;
            public static ConsoleColor DirectoryTextColor = UsableColors.Green;
            public static ConsoleColor ContentBackgroundColor = UsableColors.White;
            public static ConsoleColor DefaultBackgroundColor = UsableColors.Blue;
            public static ConsoleColor TextBoxBackgroundColor = UsableColors.White;
            public static ConsoleColor TextBoxTextColor = UsableColors.Black;
            public static ConsoleColor ErrorBackgroundColor = UsableColors.DarkRed;
            public static ConsoleColor ErrorTextColor = UsableColors.White;
            public static ConsoleColor SuccessBackgroundColor = UsableColors.Green;
            public static ConsoleColor SuccessTextColor = UsableColors.White;
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

        public static readonly Dictionary<string, ConsoleColor> strckey = new Dictionary<string, ConsoleColor>() {
            { "Black",Black },
            { "DarkBlue",DarkBlue },
            { "DarkCyan",DarkCyan },
            { "DarkRed",DarkRed },
            { "DarkMagenta",DarkMagenta },
            { "DarkYellow",DarkYellow },
            { "Gray",Gray },
            { "DarkGray",DarkGray },
            { "Blue",Blue },
            { "Green",Green },
            { "Cyan",Cyan },
            { "Red",Red },
            { "Magenta",Magenta },
            { "Yellow",Yellow },
            { "White",White },
        };
    }
}