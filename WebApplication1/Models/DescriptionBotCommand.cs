using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace WebApplication1.Models.Commands
{
    //public class DescriptionBotCommand : Command
    //{
    //    public override string Name => @"Что ты умеешь?";

    //    public override bool Contains(Message message)
    //    {
    //        if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
    //            return false;

    //        return message.Text.Contains(this.Name);
    //    }

    //    public override async Task Execute(Message message, TelegramBotClient botClient)
    //    {
    //        var chatId = message.Chat.Id;
    //        await botClient.SendTextMessageAsync(chatId, "Пока ничего :(", parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
    //    }
    //}
}
