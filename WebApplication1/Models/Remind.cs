using System;
using System.Collections.Generic;
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
            var splitedMessade = message.Split(':')[1]
                .Trim()
                .Split('-')
                .Select(x => x.Trim())
                .ToArray();
            var splitedDate = splitedMessade[0].Split(' ', '.');
            var dateArray = splitedDate.Select(x => int.Parse(x)).ToArray();
            var date = new DateTime(dateArray[2], dateArray[1], dateArray[0], dateArray[3], dateArray[4], 0);
            return new Tuple<string, DateTime>(splitedMessade[1], date);
        }
    }

}
