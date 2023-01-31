using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Notiflex.Core.Services.Helpers
{
    public class WeatherReportReturnType
    {
        public WeatherReportReturnType(Message message)
        {
            Message = message;
        }

        public WeatherReportReturnType(Message message, FileStream? fileStream)
        {
            Message = message;
            FileStream = fileStream;
        }

        public Message Message { get; set; } = null!;
        public FileStream? FileStream { get; set; }
    }
}
