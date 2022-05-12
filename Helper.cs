using System;
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
        internal const decimal HEIGHT = ((decimal)WINDOW_HEIGHT / 100) * 60;

        /// <summary>
        /// Обновление коммандной строки
        /// </summary>
        internal static void UpdateConsole()
        {
            DrawConsole(Functions.currentDir, 0, WINDOW_HEIGHT - 4, WINDOW_WIDTH, 3);
            ProcessEnterCommand(WINDOW_WIDTH);
        } //Done

        /// <summary>
        /// Обновление окна информации(второе окно)
        /// </summary>
        internal static void UpdateInfoWindow()
        {
            DrawWindow(0, (int)HEIGHT, WINDOW_WIDTH, (WINDOW_HEIGHT - 4) - (int)HEIGHT);
        } //Done

        /// <summary>
        /// Получение позиции курсора
        /// </summary>
        /// <returns></returns>
        static internal (int left, int top) GetCursorPosition()
        {
            return (Console.CursorLeft, Console.CursorTop);
        } //Done

        /// <summary>
        /// Отображение ввода командной строки
        /// </summary>
        /// <param name="width"></param>
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

                (int currentLeft, int currentTop) = GetCursorPosition();

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

        /// <summary>
        /// Обработка коммандной строки
        /// </summary>
        /// <param name="command"></param>
        internal static void ParseCommandString(string command)
        {
            UpdateInfoWindow();
            int position = (int)HEIGHT + 1;

            string[] commandParams = command.ToLower().Split(' ');
            if (commandParams.Length > 0)
            {
                switch (commandParams[0])
                {
                    case "cd":
                        Functions.ChangeDir(commandParams);
                        Functions.DirView(commandParams);
                        DrawInfo(commandParams[1]);

                        break;

                    case "ls":
                        Functions.List(commandParams);

                        break;

                    case "del":
                        {
                            if (commandParams.Length > 1)
                            {
                                Console.SetCursorPosition(2, position);
                                Console.Write($"Удалить {commandParams[1]} ?(y/n)");
                                DrawConsole(Functions.currentDir, 0, WINDOW_HEIGHT - 4, WINDOW_WIDTH, 3);
                                if (Console.ReadLine() == "y")
                                {
                                    Console.SetCursorPosition(2, position + 2);
                                    Console.WriteLine(Functions.DelFileOrDirectory(commandParams));

                                }
                                else
                                    UpdateInfoWindow();
                            }
                        }

                        break;

                    case "cp":
                        {
                            if (commandParams.Length == 3)
                            {
                                Console.SetCursorPosition(2, position + 2);

                                if (Directory.Exists(commandParams[1]))
                                {
                                    DirectoryInfo dirInfo = new DirectoryInfo(commandParams[1]);
                                    string destinationDir = commandParams[2] + "\\" + dirInfo.Name;

                                    Console.WriteLine(Functions.CopyDir(commandParams[1], destinationDir));
                                }
                                if (File.Exists(commandParams[1]) && Directory.Exists(commandParams[2]))
                                {
                                    Console.WriteLine(Functions.CopyFile(commandParams[1], commandParams[2]));
                                }
                            }
                            else
                                UpdateConsole();
                        }

                        break;

                    case "mv":
                        {
                            if (commandParams.Length == 3)
                            {
                                Console.SetCursorPosition(2, position + 2);

                                if (Directory.Exists(commandParams[1]))
                                {
                                    DirectoryInfo dirInfo = new DirectoryInfo(commandParams[1]);
                                    commandParams[2] = commandParams[2] + "\\" + dirInfo.Name;

                                    Console.WriteLine(Functions.MoveDirOrFile(commandParams));
                                }
                                if (File.Exists(commandParams[1]) && Directory.Exists(commandParams[2]))
                                {
                                    Console.WriteLine(Functions.MoveDirOrFile(commandParams));
                                }
                            }
                            else
                                UpdateConsole();
                        }

                        break;

                    case "inf":
                        {
                            if (commandParams.Length > 1)
                            {
                                DrawInfo(commandParams[1]);
                            }
                        }

                        break;
                }

            }
            UpdateConsole();
        }

        /// <summary>
        /// Отрисовка информации о файле/директории
        /// </summary>
        /// <param name="name"></param>
        internal static void DrawInfo(string name)
        {
            int position = (int)HEIGHT + 1;
            int i = 0;
            foreach (var info in Functions.NameInfo(name))
            {
                Console.SetCursorPosition(2, position + i);
                Console.Write(info);
                i += 2;
            }
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

            DrawWindow(0, 0, WINDOW_WIDTH, (int)HEIGHT);
            (int currentLeft, int currentTop) = GetCursorPosition();
            int pageLines = (int)HEIGHT - 2;
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
            Console.SetCursorPosition(WINDOW_WIDTH / 2 - footer.Length / 2, pageLines + 1);
            Console.WriteLine(footer);
        }

        /// <summary>
        /// Отрисовка состава текущей директории
        /// </summary>
        /// <param name="commandParams"></param>
        /// <param name="page"></param>
        internal static void ViewDirInfo(string commandParams, int page)
        {
            StringBuilder dirView = new StringBuilder();
            StringBuilder fileView = new StringBuilder();
            string dirName = commandParams;
            var directory = new DirectoryInfo(dirName);

            Functions.GetDirView(dirView, fileView,  directory);

            DrawWindow(0, 0, WINDOW_WIDTH, (int)HEIGHT);
            (int currentLeft, int currentTop) = GetCursorPosition();

            if (directory.Exists)
            {
                int pageLines = (int)HEIGHT - 2;
                string[] dirLines = dirView.ToString().Split(new char[] { '\n' });
                string[] fileLines = fileView.ToString().Split(new char[] { '\n' });
                int pageTotal = (dirLines.Length + pageLines - 1) / pageLines;
                if (page > pageTotal)
                    page = pageTotal;

                for (int i = (page - 1) * pageLines, counter = 0; i < page * pageLines; i++, counter++)
                {
                    if (dirLines.Length - 1 > i)
                    {
                        Console.SetCursorPosition(currentLeft + 1, currentTop + 1 + counter);
                        Console.Write(dirLines[i]);
                    }
                    if (fileLines.Length - 1 > i)
                    { 
                        Console.SetCursorPosition(WINDOW_WIDTH / 2 + 1, currentTop + 1 + counter);
                        Console.Write(fileLines[i]);
                    }
                }

                string footer = $"╡ {page} of {pageTotal} ╞";
                Console.SetCursorPosition(WINDOW_WIDTH / 2 - footer.Length / 2, pageLines + 1);
                Console.WriteLine(footer);
            }
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
