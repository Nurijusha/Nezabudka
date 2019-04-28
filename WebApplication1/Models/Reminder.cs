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
            if (remind.Date < (DateTime.UtcNow + rusTime))
            {
                await botClient.SendTextMessageAsync(chatId, "Данное время истекло");
                return;
            }
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
                    lock (AllReminds)
                    {
                        AllReminds.Add(remind.Date, remind);
                    }
                    tokenSource.Cancel();
                }
                await Task.Factory.StartNew(() => SendReminds(AllReminds, token, botClient, Send));
        }

        public static void SendReminds(SortedList<DateTime, Remind> reminds, CancellationToken ct, TelegramBotClient botClient, 
            Action<TelegramBotClient, Remind> Send)
        {
            while (reminds.Count != 0)
            {
                var remind = reminds.First();
                var interval = remind.Key - (DateTime.UtcNow + rusTime);
                if (interval <= TimeSpan.Zero)
                {
                    interval = TimeSpan.Zero;
                }
                Task.Delay(interval, ct)
                    .ContinueWith(x => Send(botClient, remind.Value), ct)
                    .Wait(ct);
                lock (reminds) { reminds.Remove(remind.Key); }
            }
        }
    }
}
