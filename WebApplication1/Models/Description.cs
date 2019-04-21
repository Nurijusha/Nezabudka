using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace WebApplication1.Models.Commands
{
    public class Description : Command
    {
        private List<string> various = new List<string>() { "что ты умеешь?", "что умеешь?", @"/description", "расскажи о себе" };
        public override string Name => @"/description";

        public override bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;

            return message.Text.Contains(various.Where(x => x == message.Text.ToLower()).First());
        }

        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;
            await botClient.SendTextMessageAsync(chatId, "Пока ничего :(", parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
        }
    }
}
