using Microsoft.Extensions.Configuration;
using Notiflex.Core.Models.APIModels;
using Notiflex.Core.Services.Contracts;
using Notiflex.Core.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Notiflex.Core.Services.BotServices
{
    public class MessageSender : IMessageSender
    {
        private readonly IConfiguration config;

        public MessageSender(IConfiguration config)
        {
            this.config = config;
        }

        public async Task SendMessage(Message message, string chatId)
        {
            TelegramBotClient bot = new(config.GetValue<string>("Notiflex_botId"));
            
            if(message == null)
            {
                throw new ArgumentException("null message");
            }

            await bot.SendTextMessageAsync(chatId, text: message.Text
                ?? throw new ArgumentException("Invalid message."));

            if (message.Location == null)
            {
                throw new ArgumentException("Invalid Location");
            }
            
            await bot.SendLocationAsync(chatId, message.Location.Latitude, message.Location.Longitude);
        }
        public async Task SendMessageWithFile(WeatherReportReturnType report, string chatId)
        {
            TelegramBotClient bot = new(config.GetValue<string>("Notiflex_botId"));

            if (report == null || report.Message == null)
            {
                throw new ArgumentException("null message");
            }

            await bot.SendTextMessageAsync(chatId, text: report.Message.Text
                ?? throw new ArgumentException("Invalid message."));

            if (report.FileStream == null)
            {
                throw new ArgumentException("Invalid Photo");
            }

            await bot.SendPhotoAsync(chatId, new Telegram.Bot.Types.InputFiles.InputOnlineFile(report.FileStream, "kur"));


            if (report.Message.Location == null)
            {
                throw new ArgumentException("Invalid Location");
            }

            await bot.SendLocationAsync(chatId, report.Message.Location.Latitude, report.Message.Location.Longitude);
        }
    }
}
