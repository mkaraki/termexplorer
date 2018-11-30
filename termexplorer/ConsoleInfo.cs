using System;
using System.Collections.Generic;
using System.Text;

namespace termexplorer
{
    class ConsoleInfo
    {
        public static void UpdateConsoleInfo()
        {
            WritableWidth = Console.WindowWidth - 1;
            WritableHeight = Console.WindowHeight;
        }

        public static int WritableWidth;
        public static int WritableHeight;

    }
}
