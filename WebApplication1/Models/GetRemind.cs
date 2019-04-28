﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NezabudkaHelperBot.Models.Commands
{
    public class GetRemind : Command
    {
        public override string Name => "";

        public override bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;
            return message.Text == "Выведи количество напоминаний";
        }

        public async override Task Execute(Message message, TelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;
                if (Reminder.AllReminds.Count() == 0)
                {
                    await botClient.SendTextMessageAsync(chatId, @"Список пуст");
                }
                else
                {
                    var count = Reminder.AllReminds.Count();
                    var str = "В списке содержится: " + count.ToString();
                    await botClient.SendTextMessageAsync(chatId, str);
                }
        }
    }
}
