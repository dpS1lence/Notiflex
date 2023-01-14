using Microsoft.Extensions.Configuration;
using Notiflex.Core.Services.Contracts;
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

        public async Task<string> SendMessage(string message, string chatId)
        {
            TelegramBotClient bot = new(config.GetValue<string>("Notiflex_botId"));

            try
            {
                Message msg = await bot.SendTextMessageAsync(chatId, message);

                return msg.Text ?? throw new ArgumentException("Error.");
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
