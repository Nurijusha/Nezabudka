using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Linq;
using System.Collections.Generic;

namespace WebApplication1.Models.Commands
{
    public class StartCommand : Command
    {
        private List<string> various = new List<string>() { "привет!", "привет", "хей", "хей!", "hello", "hello!", @"/start" };
        public override string Name => @"/start";

        public override bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;

            return message.Text.Contains(various.Where(x => x == message.Text.ToLower()).First());
        }

        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;
            await botClient.SendTextMessageAsync(chatId, "Привет!", parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
        }
    }

    public class DescriptionBotCommand : Command
    {
        private List<string> various = new List<string>() {"что ты умеешь?", "что умеешь?", @"/description", "расскажи о себе" };
        public override string Name => "Что ты умеешь?";

        public override bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;

            //return message.Text.Contains(this.Name);
            return message.Text.Contains(various.Where(x => x == message.Text).First());
        }

        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;
            await botClient.SendTextMessageAsync(chatId, "Пока ничего :(", parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
        }
    }
}