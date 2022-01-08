using System;
using System.Collections.Generic;
using System.Text;
using TelegramParser.ViewModels.Base;

namespace TelegramParser.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        private int logsCount = 0;
        public int LogsCount
        {
            get { return logsCount; }
            set { Set(ref logsCount, value); }
        }

        private int withTelegram = 0;
        public int WithTelegram
        {
            get { return logsCount; }
            set { Set(ref logsCount, value); }
        }

        private int withoutTelegram = 0;
        public int WithoutTelegram
        {
            get { return logsCount; }
            set { Set(ref logsCount, value); }
        }

        private string title = "Telegram parser";
        public string Title
        {
            get { return title; }
            set { Set(ref title, value); }
        }
    }
}
