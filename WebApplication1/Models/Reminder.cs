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
            User = message.From;
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

    public class Reminder : Command
    {
        public override string Name => "";

        public static List<Remind> AllReminds = new List<Remind>();

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
            var remind = new Remind(message);
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            Action<TelegramBotClient, Remind> Send = (client, r) => client.SendTextMessageAsync(r.Message.Chat.Id, r.Event).GetAwaiter().GetResult();

            if (remind.Date < DateTime.Now)
            {
                var chatId = message.Chat.Id;
                await botClient.SendTextMessageAsync(chatId, @"Данное время истекло!");
                return;
            }
            else
            {
                if (AllReminds.Count == 0)
                {
                    lock (AllReminds)
                    {
                        AllReminds.Add(remind);
                    }
                }
                else
                {
                    tokenSource.Cancel();
                    lock (AllReminds)
                    {
                        AllReminds.Add(remind);
                        AllReminds.OrderBy(x => x.Date);
                    }
                }
#pragma warning disable CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до завершения вызова
                Task.Factory.StartNew(() => SendReminds(AllReminds, token, botClient, Send));
#pragma warning restore CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до завершения вызова
            }
        }

        private static void SendReminds(List<Remind> Allreminds, CancellationToken ct, TelegramBotClient botClient, 
            Action<TelegramBotClient, Remind> Send)
        {
            while (AllReminds.Count != 0)
            {
                var interval = Allreminds[0].Date - DateTime.Now;
                Task.Delay(interval, ct)
                    .ContinueWith(x => Send(botClient, Allreminds[0]), ct);
                lock (AllReminds) { Allreminds.RemoveAt(0); }
            }
        }
    }
}
