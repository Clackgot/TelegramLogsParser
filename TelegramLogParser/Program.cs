using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TelegramLogParser
{



    class Program
    {
        public static IEnumerable<string> GetDirsWithTelegram(string path = @"C:\Users\Anicate\Downloads\logs2")
        {
            string[] logs = new string[0];
            try
            {
                logs = Directory.GetDirectories(path);
            }
            catch
            {
                Console.WriteLine("Не удалось получить список директорий");
                yield break;
            }

            foreach (var log in logs)
            {
                bool isTelegramExists = Directory.Exists($"{log}\\Telegram");
                if (isTelegramExists)
                {
                    yield return log;
                }
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine(GetDirsWithTelegram().Count());
            foreach (var item in GetDirsWithTelegram())
            {
                Console.WriteLine(item);
            }
        }
    }
}
