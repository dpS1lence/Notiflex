using Notiflex.Infrastructure.Data.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.UnitTests.Common.DataStorages
{
    public class UsersDataStorage
    {
        private static readonly NotiflexUser notiflexUserDefault = new()
        {
            Id = "1",
            FirstName = "Pesho",
            LastName = "Peshov",
            Age = "02-12-2012",
            Description = "Hi i am Pesho!",
            Gender = "Male",
            ProfilePic = "image.url",
            TelegramInfo = "1234",
            DefaultTime = "12:20",
            HomeTown = "Sofia",
            UserName = "peshkata",
            Email = "pesho@abc.cba",
            EmailConfirmed = true
        };

        private static readonly NotiflexUser notiflexUserNoTelegramInfo = new()
        {
            Id = "1",
            FirstName = "Pesho",
            LastName = "Peshov",
            Age = "02-12-2012",
            Description = "Hi i am Pesho!",
            Gender = "Male",
            ProfilePic = "image.url",
            TelegramInfo = null,
            DefaultTime = "12:20",
            HomeTown = "Sofia",
            UserName = "peshkata",
            Email = "pesho@abc.cba",
            EmailConfirmed = true
        };

        private static readonly NotiflexUser notiflexUserNoHomeTown = new()
        {
            Id = "1",
            FirstName = "Pesho",
            LastName = "Peshov",
            Age = "02-12-2012",
            Description = "Hi i am Pesho!",
            Gender = "Male",
            ProfilePic = "image.url",
            TelegramInfo = "1234",
            DefaultTime = "12:20",
            HomeTown = null,
            UserName = "peshkata",
            Email = "pesho@abc.cba",
            EmailConfirmed = true
        };

        private static readonly NotiflexUser notiflexUserEmailNotConfirmed = new()
        {
            Id = "1",
            FirstName = "Pesho",
            LastName = "Peshov",
            Age = "02-12-2012",
            Description = "Hi i am Pesho!",
            Gender = "Male",
            ProfilePic = "image.url",
            TelegramInfo = "1234",
            DefaultTime = "12:20",
            HomeTown = "Sofia",
            UserName = "peshkata",
            Email = "pesho@abc.cba",
            EmailConfirmed = false
        };

        private static readonly NotiflexUser notiflexUserNoEmail = new()
        {
            Id = "1",
            FirstName = "Pesho",
            LastName = "Peshov",
            Age = "02-12-2012",
            Description = "Hi i am Pesho!",
            Gender = "Male",
            ProfilePic = "image.url",
            TelegramInfo = "1234",
            DefaultTime = "12:20",
            HomeTown = "Sofia",
            UserName = "peshkata",
            Email = null,
            EmailConfirmed = true
        };

        public static NotiflexUser NotiflexUserDefault { get => notiflexUserDefault; }

        public static NotiflexUser NotiflexUserNoTelegramInfo { get => notiflexUserNoTelegramInfo; }

        public static NotiflexUser NotiflexUserNoHomeTown { get => notiflexUserNoHomeTown; }

        public static NotiflexUser NotiflexUserEmailNotConfirmed { get => notiflexUserEmailNotConfirmed; }

        public static NotiflexUser NotiflexUserNoEmail { get => notiflexUserNoEmail; }
    }
}
