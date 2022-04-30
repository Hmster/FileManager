﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileManager
{
    internal static class Helper
    {
        internal const int WINDOW_WIDTH = 140;
        internal const int WINDOW_HEIGHT = 40;


        internal static void UpdateConsole()
        {
            DrawConsole(Functions.currentDir, 0, WINDOW_HEIGHT - 4, WINDOW_WIDTH, 3);
            ProcessEnterCommand(WINDOW_WIDTH);
        }


        static internal (int left, int top) GetCursorPosition()
        {
            return (Console.CursorLeft, Console.CursorTop);
        }


        internal static void ProcessEnterCommand(int width)
        {
            (int left, int top) = GetCursorPosition();
            StringBuilder command = new StringBuilder();
            char key;
            do
            {
                key = Console.ReadKey().KeyChar;

                if (key != 8 && key != 13)
                    command.Append(key);

                (int currentLeft, int currenTtop) = GetCursorPosition();

                if (currentLeft == width - 2)
                {
                    Console.SetCursorPosition(currentLeft - 1, top);
                    Console.Write(" ");
                    Console.SetCursorPosition(currentLeft - 1, top);
                }
                if (key == (char)8/*BackSpace*/)
                {
                    if (command.Length > 0)
                        command.Remove(command.Length - 1, 1);
                    if (currentLeft >= left)
                    {
                        Console.SetCursorPosition(currentLeft, top);
                        Console.Write(" ");
                        Console.SetCursorPosition(currentLeft, top);
                    }
                    else
                    {
                        Console.SetCursorPosition(left, top);
                    }
                }
            }
            while (key != (char)13/*Enter*/);

            ParseCommandString(command.ToString());
        }

        internal static void ParseCommandString(string command)
        {
            string[] commandParams = command.ToLower().Split(' ');
            if (commandParams.Length > 0)
            {
                switch (commandParams[0])
                {
                    case "cd":
                        Functions.ChangeDir(commandParams);

                        break;

                    case "ls":
                        Functions.List(commandParams);

                        break;
                }

            }
            UpdateConsole();
        }

        /// <summary>
        /// Отрисовка дерева каталогов
        /// </summary>
        /// <param name="dir">Директория</param>
        /// <param name="page">Страница</param>
        internal static void DrawTree(DirectoryInfo dir, int page)
        {
            StringBuilder tree = new StringBuilder();
            Functions.GetTree(tree, dir, "", true);

            DrawWindow(0, 0, WINDOW_WIDTH, 18);
            (int currentLeft, int currentTop) = GetCursorPosition();
            int pageLines = 16;//TODO: зависимость от размера окна
            string[] lines = tree.ToString().Split(new char[] { '\n' });
            int pageTotal = (lines.Length + pageLines - 1) / pageLines;
            if (page > pageTotal)
                page = pageTotal;

            for (int i = (page - 1) * pageLines, counter = 0; i < page * pageLines; i++, counter++)
            {
                if (lines.Length - 1 > i)
                {
                    Console.SetCursorPosition(currentLeft + 1, currentTop + 1 + counter);
                    Console.WriteLine(lines[i]);
                }
            }

            //footer
            string footer = $"╡ {page} of {pageTotal} ╞";
            Console.SetCursorPosition(WINDOW_WIDTH / 2 - footer.Length / 2, 17);
            Console.WriteLine(footer);
        }

        

        /// <summary>
        /// Отрисовка консоли
        /// </summary>
        /// <param name="dir">Текущая папка</param>
        /// <param name="x">Начало по X</param>
        /// <param name="y">Начало по Y</param>
        /// <param name="width">Ширина</param>
        /// <param name="height">Высота</param>
        internal static void DrawConsole(string dir, int x, int y, int width, int height)
        {
            DrawWindow(x, y, width, height);
            Console.SetCursorPosition(x + 1, y + height / 2);
            Console.Write($"{dir}>");
        }

        /// <summary>
        /// Отрисовка окна
        /// </summary>
        /// <param name="x">Начало по X</param>
        /// <param name="y">Начало по Y</param>
        /// <param name="width">Ширина</param>
        /// <param name="height">Высота</param>
        internal static void DrawWindow(int x, int y, int width, int height)
        {
            int a = Console.WindowWidth;
            Console.SetCursorPosition(x, y);
            //header
            Console.Write("╔");
            for (int i = 0; i < width - 2; i++)
                Console.Write("═");
            Console.Write("╗");

            Console.SetCursorPosition(x, y + 1);
            for (int i = 0; i < height - 2; i++)
            {
                Console.Write("║");
                for (int j = x + 1; j < x + width - 1; j++)
                {
                    Console.Write(" ");
                }
                Console.Write("║");
                if( width != a)
                    Console.WriteLine();
            }

            //footer
            Console.Write("╚");
            for (int i = 0; i < width - 2; i++)
                Console.Write("═");
            Console.Write("╝");
            Console.SetCursorPosition(x, y);
        }
    }
}