﻿using Get_Unicode_EastAsianWidth;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;
using static termexplorer.Config.ColorMap;
using static termexplorer.ConsoleInfo;

namespace termexplorer
{
    class BoxWriter
    {
        public static void PopScreen(string ErrorTitle, string ErrorDetails)
        {
            UIWriter.ClearLast();

            ForegroundColor = DefaultTextColor;
            BackgroundColor = DefaultBackgroundColor;

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

        public static void Splash(string Text)
        {
            UIWriter.ClearLast();

            ForegroundColor = DefaultTextColor;
            BackgroundColor = DefaultBackgroundColor;

            Clear();
            int This_top = (WritableHeight / 2) - 1;

            if (Config.WriteProductName)
            {
                string titlepad = new string(' ', (WritableWidth - ProductInfo.Name.Length) / 2);
                Write(titlepad + ProductInfo.Name + titlepad + Environment.NewLine);
            }

            SetCursorPosition(0, This_top);
            string Pad = new string(' ', (WritableWidth - Text.Length) / 2);
            Write(Pad + Text + Pad);
        }

        public static string AskToUserScreen(string Title, string Details)
        {
            Clear();
            UIWriter.ClearLast();

            ForegroundColor = DefaultTextColor;
            BackgroundColor = DefaultBackgroundColor;

            int This_top = (WritableHeight / 2) - 3;

            if (Config.WriteProductName)
            {
                string titlepad = new string(' ', (WritableWidth - ProductInfo.Name.Length) / 2);
                WriteLine(titlepad + ProductInfo.Name + titlepad);
            }

            SetCursorPosition(0, This_top);
            string TargNamepad = new string(' ', (WritableWidth - Title.Length) / 2);
            WriteLine(TargNamepad + Title + TargNamepad);

            WriteLine();
            WriteLine();

            WriteLine(Details);

            BackgroundColor = ConsoleColor.White;
            ForegroundColor = ConsoleColor.Black;

            Write(new string(' ', WritableWidth));
            SetCursorPosition(0, CursorTop);

            return ReadLine();
        }

        public static bool CheckScreen(string Title, string Details,bool Default=false)
        {
            UIWriter.ClearLast();

            bool CurrentSelected = false;

            ForegroundColor = DefaultTextColor;
            BackgroundColor = DefaultBackgroundColor;

            while (true)
            {
                UpdateConsoleInfo();

                Clear();
                int This_top = (WritableHeight / 2) - 4;

                if (Config.WriteProductName)
                {
                    string titlepad = new string(' ', (WritableWidth - ProductInfo.Name.Length) / 2);
                    Write(titlepad + ProductInfo.Name + titlepad + Environment.NewLine);
                }

                SetCursorPosition(0, This_top);
                string ErrTitlepad = new string(' ', GetPad(Title, WindowWidth - 1) / 2);
                Write(ErrTitlepad + Title + ErrTitlepad + Environment.NewLine);

                SetCursorPosition(0, This_top + 2);
                WriteLine(Details);

                WriteLine();
                WriteLine();

                if (CurrentSelected == true) Write(">"); else Write(" ");
                WriteLine(" Yes");
                if (CurrentSelected == false) Write(">"); else Write(" ");
                WriteLine(" No");

                ConsoleKeyInfo cki = ReadKey(false);
                if (cki.Key == ConsoleKey.Enter)
                    return CurrentSelected;
                else if (cki.Key == ConsoleKey.UpArrow || cki.Key == ConsoleKey.DownArrow)
                    CurrentSelected = !CurrentSelected;
            }
        }

        private static int GetPad(string Orig, int Width)
        {
            return Width - EAWCheck.GetStrLenWithEAW(Orig, true);
        }
    }
}
