using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NezabudkaHelperBot.Models.Commands
{
    public class Reminder : Command
    {
        private static TimeSpan rusTime = new TimeSpan(3, 0, 0);
        public override string Name => "";

        public static SortedList<DateTime, Remind> AllReminds = new SortedList<DateTime, Remind>();

        public override bool Contains(Message message)
        {
            return message.Text.StartsWith("Создать напоминание");

        }

        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;
            try
            {
                Remind.SplitMessage(message.Text);
            }
            catch
            {
                await botClient.SendTextMessageAsync(chatId, @"Неверный формат сообщения. Если вы хотите создать напоминание, напишите время и событие в формате <Создать напоминание: DD.MM.YYYY HH.MI - <событие>>.");
                return;
            }
            var remind = new Remind(message);
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            Action<TelegramBotClient, Remind> Send = (client, r) =>
                {
                    var Id = r.Message.Chat.Id;
                    client.SendTextMessageAsync(Id, r.Event).GetAwaiter().GetResult();
                };

                if (AllReminds.Count == 0)
                {
                    lock (AllReminds)
                    {
                        AllReminds.Add(remind.Date, remind);
                    }
                }
                else
                {
                    tokenSource.Cancel();
                    lock (AllReminds)
                    {
                        AllReminds.Add(remind.Date, remind);
                        //AllReminds.OrderBy(x => x.Date).ToList();
                    }
                }
#pragma warning disable CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до завершения вызова
                Task.Factory.StartNew(() => SendReminds(AllReminds, token, botClient, Send));
#pragma warning restore CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до завершения вызова
        }

        public static void SendReminds(SortedList<DateTime, Remind> reminds, CancellationToken ct, TelegramBotClient botClient, 
            Action<TelegramBotClient, Remind> Send)
        {
            while (reminds.Count != 0)
            {
                var remind = reminds.First();
                var interval = remind.Key - (DateTime.UtcNow + rusTime);
                if (interval < TimeSpan.Zero)
                {
                    //botClient.SendTextMessageAsync(remind.Message.Chat.Id, "Данное время истекло").GetAwaiter().GetResult();
                    //lock (reminds) { reminds.RemoveAt(0); }
                    //continue;
                    interval = TimeSpan.Zero;
                }
                Task.Delay(interval, ct)
                    .ContinueWith(x => Send(botClient, remind.Value), ct)
                    .Wait(ct);
                lock (reminds) { reminds.RemoveAt(0); }
            }
        }
    }
}
