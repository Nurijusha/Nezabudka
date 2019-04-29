using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NezabudkaHelperBot.Models.Commands
{
    public class HowToCreateRemindCommand : Command
    {
        private List<string> various = new List<string>() { "установить таймер", "создать напоминание", @"/create_notification"};
        public override string Name => "";

        public override bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;
            return various.Where(x => message.Text.Contains(x)).Any();
        }

        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;
            await botClient.SendTextMessageAsync(chatId, @"Напишите время и событие в формате <Создать напоминание: DD.MM.YYYY HH.MI - <событие>>.");
        }
    }
}
