using System;

namespace termexplorer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

#if !DEBUG
            try
            {
#endif
                ConfigManager.InitConfig();

                ConsoleInfo.UpdateConsoleInfo();
                if (!System.IO.File.Exists(Config.Program_Exec_Path))
                {
                    BoxWriter.PopScreen("File Missing!", $"Important file is missing\nFile: termexplorer - Exec (\"{Config.Program_Exec_Path}\")", BoxWriter.InfoType.Fatal);
                    return;
                }

                while (true)
                {
                    ConsoleInfo.UpdateConsoleInfo();
                    VisualWriter.SWrite();
                    ConsoleKeyInfo cki = Console.ReadKey(false);
                    if (cki.Modifiers.HasFlag(ConsoleModifiers.Control) && cki.Key == ConsoleKey.F4) break;
                    else if (cki.Modifiers.HasFlag(ConsoleModifiers.Control) && cki.Modifiers.HasFlag(ConsoleModifiers.Shift) && cki.Key == ConsoleKey.I) DebugLogViewer();
                    UserControl.Handle(cki);
                }
#if !DEBUG
            }
            catch (Exception ex)
            {
                Logging(LogLevel.UnExpectedExceptionThrown, $"Unhandled Exception is thrown ({ex.Message} in {ex.Source}). Program will be close");
            }
            finally
            {
                SaveLog();
            }
#endif
        }

        private static void DebugLogViewer()
        {
        }

        private static void SaveLog()
        {

        }

        public static void Logging(LogLevel logLevel, string log)
        {
            string loglevelStr = logLevel.ToString("g");
            string now = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss UTCzzz");
            string loglevel = $"[{now}] [{loglevelStr}] {log}";
        }

        public enum LogLevel
        {
            NonLeveled,
            Trace,
            Debug,
            StopWatch,
            Information,
            Notice,
            Caution,
            Warning,
            SecurityWarning,
            UserException,
            UnExpectedExceptionThrown,
            Error,
            SecurityError,
            FatalError,
            HALT,
            PANIC,
        }
    }
}