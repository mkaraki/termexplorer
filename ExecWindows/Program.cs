using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ExecWindows
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 1)
                    return;
                else if (args.Length < 2)
                    Process.Start(args[0]);
                else
                {
                    List<string> opt = args.ToList();
                    string app_name = args[0];
                    opt.RemoveAt(0);
                    string arg = string.Join(" ", opt);

                    Process.Start(app_name, arg);
                }
            }
            catch (Exception)
            {
                Console.Clear();
                Console.WriteLine("termexplorer - Exec");
                Console.WriteLine("Unknown Error");
                Console.WriteLine();
                Console.WriteLine("Args :");
                for (int i =0;i < args.Length; i++)
                {
                    Console.WriteLine($"Arg {i+1}: \"{args[i]}\"");
                }
            }
        }
    }
}
