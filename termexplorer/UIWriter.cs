using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;
using static termexplorer.Config.ColorMap;

namespace termexplorer
{
    class UIWriter
    {
        public static List<string> CurrentScreen = new List<string>();

        public static void ClearLast()
        {
            CurrentScreen = new List<string>();
        }

        public static void WriteScreen(List<string> Screen)
        {
            if (Screen.Count != CurrentScreen.Count)
            {
                Writer(Screen,-1);
                return;
            }

            for (int i = 0; i < Screen.Count; i++)
            {
                if (CurrentScreen[i] != Screen[i])
                    Writer(Screen, i);
            }
        }

        public static void Writer(List<string> Screen,int wline = -1)
        {

            if (wline == -1)
            {
                Clear();
                for (int i=0;i<Screen.Count;i++)
                    LineWriter(Screen[i],i);
                CurrentScreen = Screen;
            }
            else
            {
                LineWriter(Screen[wline],wline);
                CurrentScreen[wline] = Screen[wline];
            }
        }

        public static void LineWriter(string line,int point = 0)
        {
            SetCursorPosition(0,point);

            string ccmd = null;
            string str = "";

            foreach (char eachchar in line)
            {
                if (ccmd == null && eachchar == '\\')
                {
                    Write(str);
                    str = "";
                    ccmd = "";
                    continue;
                }
                else if (ccmd != null && eachchar == ';')
                {
                    RunCmd(ccmd);
                    ccmd = null;
                    continue;
                }
                else if (ccmd != null)
                {
                    ccmd += eachchar;
                    continue;
                }

                str += eachchar;
            }
            Write(str);
        }

        public static void RunCmd(string cmd)
        {
            string[] acmd = cmd.Split(':');
            if (acmd.Length != 2)
            {
                Write(cmd);
                return;
            }

            if (acmd[0] == "break")
                return;
            else if (acmd[0] == "fgcolor")
            {
                if (!ColorCodeList.ContainsKey(acmd[1]))
                    Write(cmd);
                else
                    ForegroundColor = ColorCodeList[acmd[1]];
            }
            else if (acmd[0] == "bgcolor")
            {
                if (!ColorCodeList.ContainsKey(acmd[1]))
                    Write(cmd);
                else
                    BackgroundColor = ColorCodeList[acmd[1]];
            }
            else
                Write(cmd);
        }

        internal static void SetColorCode()
        {
            ColorCodeList = new Dictionary<string, ConsoleColor>
            {
                { "DefaultTextColor", DefaultTextColor},
                { "EntryTextColor", EntryTextColor },
                { "DirectoryTextColor", DirectoryTextColor},
                { "ContentBackgroundColor", ContentBackgroundColor},
                { "DefaultBackgroundColor", DefaultBackgroundColor},
                { "ErrorBackgroundColor", ErrorBackgroundColor},
                { "ErrorTextColor", ErrorTextColor},
            };
        }

        public static Dictionary<string, ConsoleColor> ColorCodeList = new Dictionary<string, ConsoleColor>();

        public class UIInfo
        {
            public UIInfo()
            {
                Screen = new List<string>();
            }

            public List<string> Screen;

            public void Write(string str)
            {
                if (Screen.Count == 0) Screen.Add("");
                Screen[Screen.Count - 1] += str;
            }

            public void Write(char str)
            {
                if (Screen.Count == 0) Screen.Add("");
                Screen[Screen.Count - 1] += str;
            }

            public void WriteLine(string str)
            {
                if (Screen.Count == 0) Screen.Add("");
                Screen[Screen.Count - 1] += str;
                Screen.Add("");
            }

            public void WriteLine()
            {
                Screen.Add("");
            }

            public string BackgroundColor { set {
                    if (ColorCodeList.ContainsKey(value))
                        Write($"\\bgcolor:{value};");
                } }

            public string ForegroundColor { set {
                    if (ColorCodeList.ContainsKey(value))
                        Write($"\\fgcolor:{value};");
                } }

            public void WriteDown()
            {
#if DEBUG
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();
#endif
                WriteScreen(Screen);
#if DEBUG
                sw.Stop();
                System.Diagnostics.Debug.WriteLine($"UI Write is finished in {sw.ElapsedMilliseconds}ms");
#endif
            }
        }

        public static string GetBGColorCmd(string ColorCode)
        {
            if (ColorCodeList.ContainsKey(ColorCode))
                return ($"\\bgcolor:{ColorCode};");
            else
                return null;
        }

        public static string GetFGColorCmd(string ColorCode)
        {
            if (ColorCodeList.ContainsKey(ColorCode))
                return ($"\\fgcolor:{ColorCode};");
            else
                return null;
        }
    }
}
