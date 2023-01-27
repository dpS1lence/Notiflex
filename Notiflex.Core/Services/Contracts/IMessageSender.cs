using Notiflex.Core.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Notiflex.Core.Services.Contracts
{
    public interface IMessageSender
    {
        Task SendMessageWithFile(WeatherReportReturnType report, string chatId);

        Task SendMessage(Message message, string chatId);
    }
}
