using System;

namespace termexplorer
{
    class Program
    {
        static void Main(string[] args)
        {
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
