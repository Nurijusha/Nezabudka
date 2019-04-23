using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NezabudkaHelperBot.Models.Commands
{
    public class CreateRemindCommand : Command
    {
        private List<string> various = new List<string>() { "установить таймер", "создать напоминание"};
        public override string Name => "";

        public override bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;
            return various.Where(x => x == message.Text.ToLower()).Any();
        }

        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;
            await botClient.SendTextMessageAsync(chatId, @"Хорошо. Напишите время и событие в формате DD:MM:YYYY HH:MI - <событие>.");
        }
    }
}
