using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork6._6
{
    class Program
    {
        /// <summary>
        /// Вернуть кол-во групп
        /// </summary>
        /// <param name="N"></param>
        /// <returns></returns>
        static int GetLengthGroup(int N)
        {
            return (int)Math.Log(N, 2) + 1;
        }
        /// <summary>
        /// Вывод групп чисел в консоль
        /// </summary>
        /// <param name="N"></param>
        /// <param name="GroupLength"></param>
        static void PrintGroup(int N, int GroupLength)
        {
            int i = 1;
            int j = 1;
            for (int k = 0; k < GroupLength; k++)
            {
                Console.Write($"Группа {k + 1}: ");
                while (Math.Log(i, 2) < j && i <= N)
                {
                    Console.Write($"{i}  ");
                    i++;
                }
                j++;
                Console.WriteLine();
            }
        }
        /// <summary>
        /// Сохранение групп чисел в файл
        /// </summary>
        /// <param name="N"></param>
        /// <param name="GroupLength"></param>
        /// <param name="Path"></param>
        static void SaveFileGroups(int N, int GroupLength, string Path)
        {
            using (StreamWriter stream = new StreamWriter(Path))
            {
                int i = 1;
                int j = 1;
                for (int k = 0; k < GroupLength; k++)
                {
                    while (Math.Log(i, 2) < j && i <= N)
                    {
                        stream.Write($"{i}  ");
                        i++;
                    }
                    j++;
                    stream.WriteLine();
                }
            }
        }
        /// <summary>
        /// Попытаться считать число из файла
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        static bool TryReadNumber(string Path, out int N)
        {
            try
            {
                using (StreamReader stream = new StreamReader(Path))
                {
                    if (int.TryParse(stream.ReadLine(), out N))
                    {
                        if (N >= 1 && N <= 1_000_000_000)
                            return true;
                        else
                        {
                            Console.WriteLine($"Число \'{N}\' выходит за рамки диапозона \'1 - 1_000_000_000\'");
                            return false;
                        }

                    }
                    else
                    {
                        Console.WriteLine($"Не удалось прочитать число в файле \'{Path}\'");
                        return false;
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"Файл {Path} не найден");
                N = -1;
                return false;
            }

        }
        /// <summary>
        /// Сжатие файла
        /// </summary>
        /// <param name="PathReadFile"></param>
        /// <param name="PathCompressedFile"></param>
        static void Compress(string PathReadFile, string PathCompressedFile)
        {
            using (FileStream readSream = new FileStream(PathReadFile, FileMode.OpenOrCreate))
            {
                using (FileStream targetStream = File.Create(PathCompressedFile))
                {
                    using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                    {
                        readSream.CopyTo(compressionStream);
                        Console.WriteLine("\nСжатие файла {0} завершено.\n Исходный размер: {1}  сжатый размер: {2}.",
                        PathReadFile, readSream.Length.ToString(), targetStream.Length.ToString());
                    }
                }

            }
        }
        /// <summary>
        /// Вопрос архивации
        /// </summary>
        /// <returns></returns>
        static bool QuestionCompress()
        {
            while (true)
            {
                int posX = Console.CursorLeft, posY = Console.CursorTop;
                Console.WriteLine("Поместить файл с группами в архив?");
                Console.WriteLine(" 1.Да");
                Console.WriteLine(" 2.Нет");
                switch (Console.ReadKey().KeyChar)
                {
                    case '1':
                        return true;
                    case '2':
                        return false;
                    default:
                        Console.SetCursorPosition(posX, posY);
                        StringBuilder strB = new StringBuilder();
                        Console.WriteLine(' ');
                        Console.SetCursorPosition(posX, posY);
                        Console.CursorVisible = false;
                        break;
                }
            }
        }
        static void Main(string[] args)
        {

            int n;
            bool exit = false;
            string patern = "Считанное число: {0}\n";
            patern += "1.Вывести на экран кол-во групп\n";
            patern += "2.Вывести на экран группы чисел\n";
            patern += "3.Сохранить группы чисел в файл\n";
            patern += "4.Выход\n";
            while (!exit)
            {
                if (TryReadNumber(AppContext.BaseDirectory + $@"N.txt", out n))
                {
                    Console.WriteLine(patern, n);
                    switch (Console.ReadKey().KeyChar)
                    {
                        case '1':
                            Console.WriteLine();
                            Console.WriteLine("Кол-во групп: " + GetLengthGroup(n));
                            Console.ReadKey();
                            Console.Clear();
                            break;
                        case '2':
                            Console.WriteLine();
                            PrintGroup(n, GetLengthGroup(n));
                            Console.ReadKey();
                            Console.Clear();
                            break;
                        case '3':
                            Console.WriteLine();
                            string path = AppContext.BaseDirectory + $@"\GroupsNubers{n}.txt";
                            SaveFileGroups(n, GetLengthGroup(n), path);
                            Console.WriteLine($"Файл {path} сохранен.");
                            if (QuestionCompress())
                                Compress(AppContext.BaseDirectory + $@"\GroupsNubers{n}.txt", AppContext.BaseDirectory + $@"\GroupsNubers{n}.zip");
                            Console.ReadKey();
                            Console.Clear();
                            break;
                        case '4':
                            exit = true;
                            break;
                        default:
                            Console.Clear();
                            break;

                    }
                }
                else
                {
                    Console.WriteLine("Попробуйте исправить ошибку и нажмите любую кнопку.");
                    Console.WriteLine("Для выхода из программы нажмите \'0\'");
                    if (Console.ReadKey().KeyChar == '0')
                        break;
                    Console.Clear();
                }
            }
        }
    }
}
