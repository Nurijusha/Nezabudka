using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NezabudkaHelperBot.Models.Commands
{
    public class GetRemindCommand : Command
    {
        public override string Name => "";

        public override bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;
            return message.Text.ToLower().Trim(' ') == "выведи количество напоминаний";
        }

        public async override Task Execute(Message message, TelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;
            int count = 0;
            foreach( var remind in Reminder.AllReminds)
            {
                if (remind.Value.Message.Chat.Id == chatId)
                    count++;
            }
                if (count == 0)
                {
                    await botClient.SendTextMessageAsync(chatId, @"Список пуст");
                }
                else
                {
                    var str = "В списке содержится: " + count.ToString();
                    await botClient.SendTextMessageAsync(chatId, str);
                }
        }
    }
}
