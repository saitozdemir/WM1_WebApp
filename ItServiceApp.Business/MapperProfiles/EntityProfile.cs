using AutoMapper;
using ITServiceApp.Core.Entities;
using ITServiceApp.Core.ViewModels;

namespace ITServiceApp.Business.MapperProfiles
{
    public class EntityProfile:Profile
    {
        public EntityProfile()
        {
            CreateMap<SubscriptionType, SubscriptionTypeViewModel>().ReverseMap();
        }
    }
}
