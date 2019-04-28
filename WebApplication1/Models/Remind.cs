using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NezabudkaHelperBot.Models.Commands
{
    public class Remind
    {
        public string Event { get; }
        public DateTime Date { get; }
        public Message Message { get; }
        
        public Remind(Message message)
        {
            var remind = SplitMessage(message.Text);
            Event = remind.Item1;
            Date = remind.Item2;
            Message = message;
        }

        public static Tuple<string, DateTime> SplitMessage(string message)
        {
            if (message[message.Length - 1] == '.')
                message.Remove(message.Length - 1);
            var splitedMessage = message.Split(':')[1]
                .Trim()
                .Split('-')
                .Select(x => x.Trim())
                .ToArray();
            var date = DateTime.ParseExact(splitedMessage[0], "dd.MM.yyyy HH.mm", CultureInfo.GetCultureInfo("ru-RU"));
            return new Tuple<string, DateTime>(splitedMessage[1], date);
        }
    }

}
