using Microsoft.Extensions.Configuration;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.Core.Quartz.Jobs
{
	public class CreateJob : IJob
	{
		private readonly IConfiguration config;

		public CreateJob(IConfiguration config)
		{
			this.config = config;
		}

		public async Task Execute(IJobExecutionContext context)
		{
			int a = 4;
			int b = 6;
			int c = 7;

			await Task.Run(() => { });
		}
	}
}
