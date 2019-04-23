using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NezabudkaHelperBot.Models.Commands
{
    public class Remind
    {
        public string Event { get; }
        public DateTime Date { get; }
        public Remind(string message)
        {
            var splitedMessade = message.Split(" - ");
            Event = splitedMessade[1];
            var splitedDate = splitedMessade[0].Split(' ', ':');
            var dateArray = splitedDate.Select(x => int.Parse(x)).ToArray();
            Date = new DateTime(dateArray[2], dateArray[1], dateArray[0], dateArray[3], dateArray[4], 0);
        }
    }

    public class Reminder : Command
    {
        private SortedList<Remind, DateTime> AllReminds { get; set; }
        public override string Name => "";

        public override bool Contains(Message message)
        {
            throw new NotImplementedException();
        }

        public override Task Execute(Message message, TelegramBotClient client)
        {
            throw new NotImplementedException();
        }
    }
}
