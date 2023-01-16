using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.Core.Services.Contracts
{
    public interface IWeatherApiService
    {
        Task<IWeatherApiService> GetDataAsync(string url);
    }
}
