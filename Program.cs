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
            Console.Title = Properties.Settings.Default.appName;

            Console.SetWindowSize(Helper.WINDOW_WIDTH, Helper.WINDOW_HEIGHT);
            Console.SetBufferSize(Helper.WINDOW_WIDTH, Helper.WINDOW_HEIGHT);

            Helper.DrawWindow(0, 0, Helper.WINDOW_WIDTH, (int)Helper.HEIGHT);
            Functions.DirView(Functions.currentDir);
            Helper.DrawWindow(0, (int)Helper.HEIGHT, Helper.WINDOW_WIDTH, (Helper.WINDOW_HEIGHT-4) - (int)Helper.HEIGHT);
            Helper.DrawInfo(Functions.currentDir);

            Helper.UpdateConsole();


            Console.ReadKey(true);
        }

    }
}
