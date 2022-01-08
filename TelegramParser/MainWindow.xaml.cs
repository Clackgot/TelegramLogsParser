using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TelegramParser.ViewModels;

namespace TelegramParser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }


        private List<string> dirLogsWithTelegram = new List<string>();

        public IEnumerable<string> GetDirsWithTelegram(string path)
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

        /// <summary>
        /// Событие нажатия на кнопку "Открыть папку"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonOpenDir_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            dialog.Multiselect = false;
            if (dialog.ShowDialog(this).GetValueOrDefault())
            {
                var path = dialog.SelectedPath;

                int dirsCount = Directory.GetDirectories(path).Count();

                dirLogsWithTelegram = GetDirsWithTelegram(path).ToList();
                int dirsWithTelegramCount = dirLogsWithTelegram.Count();

                ((MainWindowViewModel)DataContext).LogsCount = dirsCount;
                ((MainWindowViewModel)DataContext).WithTelegram = dirsWithTelegramCount;
                ((MainWindowViewModel)DataContext).WithoutTelegram = dirsCount - dirsWithTelegramCount;
            }


        }
        /// <summary>
        /// Событие нажатия на кнопку "Копировать найденные папки"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ButtonSaveDirs_Click(object sender, RoutedEventArgs e)
        {


            int copiedCount = 0;
            int dirLogsWithTelegramCount = dirLogsWithTelegram.Count();


            double progressBarValue = 0;

            int errorCount = 0;


            foreach (var dir in dirLogsWithTelegram)
            {
                var sourceDirectory = $"{dir}\\Telegram";
                var logName = sourceDirectory.Split('\\').TakeLast(2).First().Trim();
                var destinationDirectoryName = $"output\\{logName}";
                await Task.Run(() =>
                {
                    try
                    {
                        FileSystem.CopyDirectory(sourceDirectory, destinationDirectoryName);
                        copiedCount += 1;
                        progressBarValue = (double)(((double)copiedCount / (double)dirLogsWithTelegramCount) * 100);
                    }
                    catch(Exception e)
                    {
                        errorCount++;
                    }
                });
                progressBar.Value = progressBarValue;
            }


            if(errorCount == dirLogsWithTelegramCount)
            {
                MessageBox.Show($"Не удалось скопировать {errorCount} папок", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (errorCount > 0)
            {
                MessageBox.Show($"Cкопировано [{dirLogsWithTelegramCount-errorCount}/{dirLogsWithTelegramCount}] папок", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show($"Скопировано {dirLogsWithTelegramCount} папок", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
