using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System;

namespace NezabudkaHelperBot.Models.Commands
{
    public class ShowAllRemindsCommand : Command
    {
        public override string Name => "";

        public override bool Contains(Message message)
        {
            return message.Text == "Покажи все напоминания";
        }

        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;
            if (Reminder.AllReminds.Count() == 0)
            {
                await botClient.SendTextMessageAsync(chatId, "Список пуст");
            }
            else
            {
                var resultStr = new StringBuilder();
                foreach (var remind in Reminder.AllReminds)
                {
                    resultStr.Append(remind.Value.Date.ToString() + " - " + remind.Value.Event);
                    resultStr.Append(Environment.NewLine);
                }
                await botClient.SendTextMessageAsync(chatId, resultStr.ToString());
            }
        }
    }
}
