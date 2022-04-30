using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileManager
{
    internal static class Functions
    {
        internal static string currentDir = Directory.GetCurrentDirectory();

        /// <summary>
        /// Создание дерева каталогов
        /// </summary>
        /// <param name="tree">Строка для дерева</param>
        /// <param name="dir">Директория</param>
        /// <param name="indent"></param>
        /// <param name="lastDirectory">Проверка, последняя ли папка</param>
        internal static void GetTree(StringBuilder tree, DirectoryInfo dir, string indent, bool lastDirectory)
        {
            tree.Append(indent);
            if (lastDirectory)
            {
                tree.Append("└─");
                indent += "  ";
            }
            else
            {
                tree.Append("├─");
                indent += "│ ";
            }

            tree.Append($"{dir.Name}\n"); //<---- ПЕРЕХОД НА СЛЕД СТРОКУ

            FileInfo[] subFiles = dir.GetFiles();
            for (int i = 0; i < subFiles.Length; i++)
            {
                if (i == subFiles.Length - 1)
                {
                    tree.Append($"{indent}└─{subFiles[i].Name}\n");
                }
                else
                {
                    tree.Append($"{indent}├─{subFiles[i].Name}\n");
                }
            }

            DirectoryInfo[] subDirects = dir.GetDirectories();
            for (int i = 0; i < subDirects.Length; i++)
                GetTree(tree, subDirects[i], indent, i == subDirects.Length - 1);

        }
        /// <summary>
        /// Смена директории
        /// </summary>
        /// <param name="commandParams">строка с коммандой</param>
        internal static void ChangeDir(string[] commandParams)
        {
            if (commandParams.Length > 1 && Directory.Exists(commandParams[1]))
            {
                currentDir = commandParams[1];
            }
        }

        internal static void List(string[] commandParams)
        {
            if (commandParams.Length > 1 && Directory.Exists(commandParams[1]))
            {
                if (commandParams.Length > 3 && commandParams[2] == "-p" && int.TryParse(commandParams[3], out int n))
                {
                    Helper.DrawTree(new DirectoryInfo(commandParams[1]), n);
                }
                else
                {
                    Helper.DrawTree(new DirectoryInfo(commandParams[1]), 1);
                }
            }
        }

        /// <summary>
        /// Перемещение каталога
        /// </summary>
        /// <param name="oldPath"></param>
        /// <param name="newPath"></param>
        /// <returns></returns>
        public static string MoveDirectory(string oldPath, string newPath)
        {
            string message;
            DirectoryInfo dirInfo = new DirectoryInfo(oldPath);
            if (dirInfo.Exists && Directory.Exists(newPath))
            {
                dirInfo.MoveTo(newPath);
                message = "Каталог перемещён";
            }
            else
                message = "Каталог не существует";

            return message;
        }

        /// <summary>
        /// Удаление каталога
        /// </summary>
        /// <param name="dirName"></param>
        /// <returns></returns>
        public static string DelDirectory(string dirName)
        {
            string message;
            DirectoryInfo dirInfo = new DirectoryInfo(dirName);
            if (dirInfo.Exists)
            {
                dirInfo.Delete(true);
                message = "Каталог удален";
            }
            else

                message = "Каталог не существует";

            return message;
        }

        /// <summary>
        /// Копирование файлa
        /// </summary>
        /// <param name="path">Отсюда</param>
        /// <param name="nPath">Сюда</param>
        public static string CopypFiles(string path, string nPath)
        {
            string message;
            FileInfo fileInf = new FileInfo(path);
            if (fileInf.Exists)
            {
                File.Copy(path, nPath, true);
                message = "Файл скопирован";
            }
            else
                message = "Файл не существует";

            return message;
        }

        /// <summary>
        /// Удаление файла
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string DelFile(string filePath)
        {
            string message;

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                message = "Файл успешно удален!";
            }
            else
                message = "Файл не существует";

            return message;

        }
    }

    
}
