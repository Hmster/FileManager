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
        internal static string currentDir = Properties.Settings.Default.startingDir;


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

        } //Done

        /// <summary>
        /// Вывод дерева каталогов
        /// </summary>
        /// <param name="commandParams"></param>
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
        } //Done

        /// <summary>
        /// Смена директории
        /// </summary>
        /// <param name="commandParams">строка с коммандой</param>
        internal static void ChangeDir(string[] commandParams)
        {
            if (commandParams.Length > 1 && Directory.Exists(commandParams[1]))
            {
                currentDir = commandParams[1];
                Properties.Settings.Default.startingDir = currentDir;
                Properties.Settings.Default.Save();
            }
        } //Done

        /// <summary>
        /// Получение состава текущей директории
        /// </summary>
        /// <param name="dirView"></param>
        /// <param name="fileView"></param>
        /// <param name="dir"></param>
        internal static void GetDirView(StringBuilder dirView, StringBuilder fileView, DirectoryInfo dir)
        {
            if (dir.Exists)
            {
                dirView.AppendLine("Директории:\n");
                DirectoryInfo[] dirs = dir.GetDirectories();
                foreach (DirectoryInfo directory in dirs)
                {
                    dirView.Append(directory.Name + "\n");
                }

                fileView.AppendLine("Файлы:\n");
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo file in files)
                {
                    fileView.AppendLine(file.Name + "\n");
                }
            }
        } //Done

        /// <summary>
        /// Отображение состава директории
        /// </summary>
        /// <param name="commandParams"></param>
        internal static void DirView(string[] commandParams)
        {
            if (commandParams.Length > 1 && Directory.Exists(commandParams[1]))
            {
                if (commandParams.Length > 3 && commandParams[2] == "-p" && int.TryParse(commandParams[3], out int n))
                {
                    Helper.ViewDirInfo(commandParams[1], n);
                }
                else
                {
                    Helper.ViewDirInfo(commandParams[1], 1);
                }
            }
        } //Done

        /// <summary>
        /// Отображение состава начальной директории
        /// </summary>
        /// <param name="dir"></param>
        internal static void DirView(string dir)
        {
            Helper.ViewDirInfo(dir, 1);
        } //Done

        /// <summary>
        /// Удаление файла или каталога
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        internal static string DelFileOrDirectory(string[] commandParams)
        {
            string message;

            if (commandParams.Length > 1 && File.Exists(commandParams[1]))
            {
                File.Delete(commandParams[1]);
                message = "Файл успешно удален!";
            }
            else if (commandParams.Length > 1 && Directory.Exists(commandParams[1]))
            {
                Directory.Delete(commandParams[1], true);
                message = "Директория успешно удалена!";
            }
            else
                message = "Директория/файл не существует";

            return message;

        } //Done

        /// <summary>
        /// Копирование каталога или файла
        /// </summary>
        /// <param name="source">откуда</param>
        /// <param name="destination">куда</param>
        /// <returns>сообщение о успешном копировании</returns>
        /// <exception cref="DirectoryNotFoundException"></exception>
        internal static string CopyDir(string source, string destination)
        {
            string message;

            if (Directory.Exists(source))
            {
                var dir = new DirectoryInfo(source);

                DirectoryInfo[] dirs = dir.GetDirectories();
                Directory.CreateDirectory(destination);

                foreach (FileInfo file in dir.GetFiles())
                {
                    string targetFilePath = Path.Combine(destination, file.Name);
                    file.CopyTo(targetFilePath);
                }
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destination, subDir.Name);
                    CopyDir(subDir.FullName, newDestinationDir);
                }
                message = "Директория успешно скопирована";
            }
            else
                message = "Неверные параметры";

            return message;
        } //Done

        /// <summary>
        /// Копирование файла
        /// </summary>
        /// <param name="source">откуда</param>
        /// <param name="destination">куда</param>
        /// <returns></returns>
        internal static string CopyFile(string source, string destination)
        {
            string message;

            if (File.Exists(source) && Directory.Exists(destination))
            {
                FileInfo file = new FileInfo(source);
                string targetFilePath = Path.Combine(destination, file.Name);

                file.CopyTo(targetFilePath);

                message = "Файл успешно скопирован";
            }
            else
                message = "Неверные параметры";

            return message;
        } //Done

        /// <summary>
        /// Перемещение каталога
        /// </summary>
        /// <param name="oldPath"></param>
        /// <param name="newPath"></param>
        /// <returns></returns>
        internal static string MoveDirOrFile(string[] commandParams)
        {
            string message;

            if (File.Exists(commandParams[1]))
            {
                FileInfo file = new FileInfo(commandParams[1]);
                string targetFilePath = Path.Combine(commandParams[2], file.Name);

                file.MoveTo(targetFilePath);

                message = "Файл успешно перемещен";
            }
            else if (Directory.Exists(commandParams[1]) && !Directory.Exists(commandParams[2]))
            {
                Directory.Move(commandParams[1], commandParams[2]);

                message = "Директория успешно перемещена";
            }
            else
                message = "Директория/файл не существует";

            return message;
        } //Done

        /// <summary>
        /// Получение информации файле/директории
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal static List<string> NameInfo(string name)
        {
            List<string> info = new List<string>();
            if (Directory.Exists(name))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(name);

                info.Add($"Имя директории: {dirInfo.Name}");
                info.Add($"Корневая директория: {dirInfo.Root}");
                info.Add($"Путь к директории: {dirInfo.FullName}");
                info.Add($"Директория создана: {dirInfo.CreationTime}");
                info.Add($"Изменено: {dirInfo.LastWriteTime}");
            }
            else if (File.Exists(name))
            {
                FileInfo fileInfo = new FileInfo(name);

                info.Add($"Имя файла: {fileInfo.Name}");
                info.Add($"Путь к файлу: {fileInfo.FullName}");
                info.Add($"Время создания: {fileInfo.CreationTime}");
                info.Add($"Изменено: {fileInfo.LastWriteTime}");
                info.Add($"Размер: {fileInfo.Length} b.");
            }

            return info;
        } //Done
    }
}
