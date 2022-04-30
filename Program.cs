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

            decimal height = ((decimal)Helper.WINDOW_HEIGHT / 100) * 60;

            Helper.DrawWindow(0, 0, Helper.WINDOW_WIDTH, (int)height);
            Helper.DrawWindow(0, (int)height, Helper.WINDOW_WIDTH, (Helper.WINDOW_HEIGHT-4) - (int)height);

            Helper.UpdateConsole();


            Console.ReadKey(true);
        }

    }
}
