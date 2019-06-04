namespace MIS.WebApp.MapperConfigurations
{
    using AutoMapper;

    using Models;

    using ViewModels;

    public class MISProfile : Profile
    {
        public MISProfile()
        {
            this.CreateMap<SystemProduct, SystemProductCreateViewModel>().ReverseMap();
            this.CreateMap<SystemProduct, SystemProductShowViewModel>().ReverseMap();
            this.CreateMap<SystemProduct, SystemProductBuyViewModel>().ReverseMap();
        }
    }
}