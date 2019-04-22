using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NezabudkaHelperBot.Models.Commands
{
    public class DescriptionCommands : Command
    {
        private List<string> various = new List<string>() { "что ты умеешь", "что умеешь", @"/description", "расскажи о себе" , "че умеешь"};
        public override string Name => @"/description";

        public override bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;

            return message.Text.Contains(various.Where(x => x == message.Text).FirstOrDefault());
        }

        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;
            await botClient.SendTextMessageAsync(chatId, "Я пока ничего не умею :(", parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
        }
    }
}
