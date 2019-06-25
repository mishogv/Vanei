namespace MIS.WebApp.MapperConfigurations
{
    using AutoMapper;

    using Models;

    using ViewModels.AdministratorManage;

    public class MISProfile : Profile
    {
        public MISProfile()
        {
            this.CreateMap<MISUser, AdministratorShowUserViewModel>().ReverseMap();
        }
    }
}