﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WCDApi.Entity;
using AutoMapper;
using WCDApi.Model.Users;
using WCDApi.Model.MonitoredItems;

namespace WCDApi.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() { 
             //user
            CreateMap<User, UserModel>();
            CreateMap<RegisterModel, User>();
            CreateMap<UpdateModel, User>();
            //monitoredItem
            CreateMap<CreateItemModel, MonitoredItem>();
            CreateMap<MonitoredItem, MonitoredItemModel>();
        }
    }
}
