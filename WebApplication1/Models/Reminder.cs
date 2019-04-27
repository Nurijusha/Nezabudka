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
        public override string Name => "";

        public static List<Remind> AllReminds = new List<Remind>();

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
                    client.SendTextMessageAsync(Id, r.Event).GetAwaiter().GetResult();//?
                };

            if (remind.Date.CompareTo(DateTime.Now) < 0 || //?
                remind.Date.Date == DateTime.Now.Date && (remind.Date.Hour < DateTime.Now.Hour ||
                    remind.Date.Hour == DateTime.Now.Hour && remind.Date.Minute <= DateTime.Now.Minute))
            {
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
                        AllReminds.OrderBy(x => x.Date).ToList();
                    }
                }
#pragma warning disable CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до завершения вызова
                Task.Factory.StartNew(() => SendReminds(token, botClient, Send));
#pragma warning restore CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до завершения вызова
            }
        }

        public static void SendReminds(CancellationToken ct, TelegramBotClient botClient, 
            Action<TelegramBotClient, Remind> Send)
        {
            while (AllReminds.Count != 0)
            {
                var interval = AllReminds[0].Date - DateTime.Now;
                Task.Delay(interval, ct)
                    .ContinueWith(x => Send(botClient, AllReminds[0]), ct)
                    .Wait(ct);
                lock (AllReminds) { AllReminds.RemoveAt(0); }
            }
        }
    }
}
