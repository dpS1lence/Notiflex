using Notiflex.Core.Services.Contracts;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Notiflex.Core.Quartz.Jobs
{
    public class ReportSenderJob : IJob
    {
        private readonly IMessageSender _messageSender;
        private readonly IMessageConfigurer _messageConfigurer;

        public ReportSenderJob(IMessageSender messageSender, IMessageConfigurer messageConfigurer)
        {
            _messageSender = messageSender;
            _messageConfigurer = messageConfigurer;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap jobDataMap = context.MergedJobDataMap;
            string telegramChatId = jobDataMap.GetString("telegramChatId")!;
            string city = jobDataMap.GetString("city")!;
            string coords = (await _messageConfigurer.ConvertNameToCoordinates(city))[2];
            Message message = await _messageConfigurer.ConfigureWeatherReportMessage(coords);

            //await _messageSender.SendMessage(message, telegramChatId);

        }
    }
}
