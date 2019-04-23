using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Linq;
using System.Collections.Generic;

namespace NezabudkaHelperBot.Models.Commands
{
    public class HelloCommands : Command
    {
        private List<string> various = new List<string>() { "привет", "хей", "hello", @"/start" ,"приветик", "здравствуй", "ну здравствуй", "здравствуйте", "однако, здравствуйте", "здарова","приветули"};
        public override string Name => @"/start";

        public override bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;

            return various.Where(x => x == message.Text.ToLower()).Any();
        }

        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;
            await botClient.SendTextMessageAsync(chatId, "Привет! Спроси у меня, что я умею!");
        }
    }
}