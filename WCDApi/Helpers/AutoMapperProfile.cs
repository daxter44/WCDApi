using AutoMapper;
using WCDApi.Model.Users;
using WCDApi.Model.MonitoredItems;
using WCDApi.DataBase.Entity;

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
            CreateMap<MonitoredItemModel, MonitoredItem>();
            CreateMap<MonitoredHistoryItemModel, MonitoredHistoryItem>();
            CreateMap<MonitoredHistoryItem, MonitoredHistoryItemModel>();
        }
    }
}
