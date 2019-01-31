using System;
using System.Collections.Generic;
using System.Text;
using static termexplorer.DocumentWriter.VWriter;

namespace termexplorer.DocumentWriter
{
    class UserController
    {
        public static void Handle(ConsoleKeyInfo cki)
        {
            if (cki.Key == ConsoleKey.UpArrow)
            {
                if (ToWrite.CurrentRow > 0)
                    ToWrite.CurrentRow--;
            }
            else if (cki.Key == ConsoleKey.DownArrow)
            {
                if (ToWrite.CurrentRow < ToWrite.Rows-1)
                    ToWrite.CurrentRow++;
            }
        }
    }
}
