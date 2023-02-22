using Microsoft.AspNetCore.Identity;
using Moq;
using Notiflex.Core.Models.DTOs;
using Notiflex.Core.Services.Contracts;
using Notiflex.Infrastructure.Data.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.UnitTests.Core.Helpers
{
    public class ModelConfigurerMockProvider
    {
        public static Mock<IModelConfigurer> MockModelConfigurer()
        {
            var modelConfigurer = new Mock<IModelConfigurer>();

            modelConfigurer.Setup(x => x.ConfigureWeatherReport(It.IsAny<string>()))
                .ReturnsAsync(new WeatherCardDto());

            modelConfigurer.Setup(x => x.ConfigureForecastReport(It.IsAny<string>()))
                .ReturnsAsync(new List<WeatherCardDto>());

            modelConfigurer.Setup(x => x.ConvertNameToCoordinates(It.IsAny<string>()))
                .ReturnsAsync(new List<string>()
                {
                    "1",
                    "1",
                    "1"
                });

            return modelConfigurer;
        }
    }
}
