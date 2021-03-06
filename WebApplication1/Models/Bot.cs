﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using NezabudkaHelperBot.Models;
using NezabudkaHelperBot.Models.Commands;

namespace NezabudkaHelperBot
{
    public class Bot
    {
        private static TelegramBotClient botClient;
        private static List<Command> commandsList;

        public static IReadOnlyList<Command> Commands => commandsList.AsReadOnly();

        public static async Task<TelegramBotClient> GetBotClientAsync()
        {
            if (botClient != null)
            {
                return botClient;
            }

            commandsList = new List<Command>();
            commandsList.Add(new HelloCommands());
            commandsList.Add(new DescriptionCommands());
            commandsList.Add(new HowToCreateRemindCommand());
            commandsList.Add(new Reminder());
            commandsList.Add(new GetRemindCommand());
            commandsList.Add(new ShowAllRemindsCommand());
            botClient = new TelegramBotClient(AppSettings.Key);
            string hook = string.Format(AppSettings.Url, "api/message/update");
            await botClient.SetWebhookAsync(hook);
            return botClient;
        }
    }
}

