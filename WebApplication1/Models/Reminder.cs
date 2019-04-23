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
            var remind = SplitMessage(message);
            Event = remind.Item1;
            Date = remind.Item2;
        }

        public static Tuple<string, DateTime> SplitMessage(string message)
        {
            if (message[message.Length - 1] == '.')
                message.Remove(message.Length - 1);
            var splitedMessade = message.Split(": ")[1].Split(" - ");
            var splitedDate = splitedMessade[0].Split(' ', ':');
            var dateArray = splitedDate.Select(x => int.Parse(x)).ToArray();
            var date = new DateTime(dateArray[2], dateArray[1], dateArray[0], dateArray[3], dateArray[4], 0);
            return new Tuple<string, DateTime>(splitedMessade[1], date);
        }
    }

    public class Reminder : Command
    {
        public override string Name => "";

        public static List<Remind> AllReminds { get; set; }

        public override bool Contains(Message message)
        {
            return message.Text.StartsWith("Создать напоминание");

        }

        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            try
            {
                Remind.SplitMessage(message.Text);
            }
            catch
            {
                var chatId = message.Chat.Id;
                await botClient.SendTextMessageAsync(chatId, @"Неверный формат сообщения. Если вы хотите создать напоминание, напишите время и событие в формате <Создать напоминание: DD.MM.YYYY HH.MI - <событие>>.");
                return;
            }
            var remind = new Remind(message.Text);
            AllReminds.Add(remind);
            AllReminds.OrderBy(x => x.Date);
        }
    }
}
