using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    internal class Program
    {
        


        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Title = "FileManager";

            Console.SetWindowSize(Helper.WINDOW_WIDTH, Helper.WINDOW_HEIGHT);
            Console.SetBufferSize(Helper.WINDOW_WIDTH, Helper.WINDOW_HEIGHT);

            Helper.DrawWindow(0, 0, Helper.WINDOW_WIDTH, (int)Helper.HEIGHT);
            Helper.DrawWindow(0, (int)Helper.HEIGHT, Helper.WINDOW_WIDTH, (Helper.WINDOW_HEIGHT-4) - (int)Helper.HEIGHT);

            Helper.UpdateConsole();


            Console.ReadKey(true);
        }

    }
}
