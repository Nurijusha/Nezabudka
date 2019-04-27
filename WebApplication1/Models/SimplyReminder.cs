using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NezabudkaHelperBot.Models.Commands
{
    public class SimplyReminder : Command
    {
        public static List<Remind> AllReminds = new List<Remind>();
        public override string Name => "";

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
            if (remind.Date.CompareTo(DateTime.Now) < 0)
            {
                await botClient.SendTextMessageAsync(chatId, @"Данное время истекло!");
                return;
            }
            else
            {
                AllReminds.Add(remind);
                while(AllReminds.Count() > 0)
                {

                }
            }

        }
    }
}
