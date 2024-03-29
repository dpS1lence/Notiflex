﻿using Microsoft.AspNetCore.Identity;
using Notiflex.Infrastructure.Data.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable

namespace Notiflex.UnitTests.Common.DataStorages
{
    public class UsersDataStorage
    {
        public UsersDataStorage()
        {
            Users = new List<NotiflexUser>();
            Roles = new List<IdentityRole>();
            UserRoles = new List<IdentityUserRole<string>>();

            InstantiateUsers();
            InstantiateRoles();
            InstantiateUserRoles();
        }

        public List<NotiflexUser> Users { get; private set; }

        public List<IdentityRole> Roles { get; private set; }
        
        public List<IdentityUserRole<string>> UserRoles { get; private set; }

        public  NotiflexUser NotiflexUserDefault { get; private set; }

        public  NotiflexUser NotiflexUserNoId { get; private set; }

        public  NotiflexUser NotiflexUserApproved { get; private set; }

        public  NotiflexUser NotiflexUserNoTelegramInfo { get; private set;  }

        public  NotiflexUser NotiflexUserNoHomeTown { get; private set;  }

        public  NotiflexUser NotiflexUserEmailNotConfirmed { get; private set;  }

        public  NotiflexUser NotiflexUserNoEmail { get; private set;  }
        private void InstantiateUsers()
        {
            NotiflexUser notiflexUserDefault = new()
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
                EmailConfirmed = true,
                Triggers = new TriggersDataStorage().Triggers
            };
            NotiflexUserDefault = notiflexUserDefault;
            Users.Add(notiflexUserDefault);

            NotiflexUser notiflexUserApproved = new()
            {
                Id = "Approved",
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
                EmailConfirmed = true,
                Triggers = new TriggersDataStorage().Triggers
            };
            NotiflexUserApproved = notiflexUserApproved;
            Users.Add(notiflexUserApproved);

            NotiflexUser notiflexUserNoTelegramInfo = new()
            {
                Id = "2",
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
                EmailConfirmed = true,
                Triggers = new TriggersDataStorage().Triggers
            };
            NotiflexUserNoTelegramInfo = notiflexUserNoTelegramInfo;
            Users.Add(notiflexUserNoTelegramInfo);

            NotiflexUser notiflexUserNoHomeTown = new()
            {
                Id = "3",
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
                EmailConfirmed = true,
                Triggers = new TriggersDataStorage().Triggers
            };
            NotiflexUserNoHomeTown = notiflexUserNoHomeTown;
            Users.Add(notiflexUserNoHomeTown);

            NotiflexUser notiflexUserEmailNotConfirmed = new()
            {
                Id = "4",
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
                EmailConfirmed = false,
                Triggers = new TriggersDataStorage().Triggers
            };
            NotiflexUserEmailNotConfirmed = notiflexUserEmailNotConfirmed;
            Users.Add(notiflexUserEmailNotConfirmed);

            NotiflexUser notiflexUserNoEmail = new()
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
                EmailConfirmed = true,
                Triggers = new TriggersDataStorage().Triggers
            };
            NotiflexUserNoEmail = notiflexUserNoEmail;
            Users.Add(notiflexUserNoEmail);

            NotiflexUser notiflexUserNoId = new()
            {
                Id = null,
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
                EmailConfirmed = true,
                Triggers = new TriggersDataStorage().Triggers
            };
            NotiflexUserNoId = notiflexUserNoId;
            Users.Add(notiflexUserNoEmail);
        }
        private void InstantiateRoles()
        {
            var defaultRole = new IdentityRole()
            {
                Id = "1",
                Name = "ApprovedUser",
                NormalizedName = "APPROVEDUSER",
                ConcurrencyStamp = Guid.NewGuid().ToString()

            }; 
        }
        private void InstantiateUserRoles()
        {
            var approvedUserRole = new IdentityUserRole<string>()
            {
                UserId = "Approved",
                RoleId = "1"
            };
            UserRoles.Add(approvedUserRole);
        }
        
    }
}
