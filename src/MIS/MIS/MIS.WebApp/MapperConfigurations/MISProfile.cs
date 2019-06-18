﻿namespace MIS.WebApp.MapperConfigurations
{
    using Areas.Administrator.BindingModels;

    using AutoMapper;

    using Models;

    using ServicesModels;

    using ViewModels;

    public class MISProfile : Profile
    {
        public MISProfile()
        {
            this.CreateMap<SystemProduct, SystemProductCreateInputModel>().ReverseMap();
            this.CreateMap<SystemProductServiceModel, SystemProductCreateInputModel>().ReverseMap();
            this.CreateMap<SystemProduct, SystemProductShowViewModel>().ReverseMap();
            this.CreateMap<SystemProduct, SystemProductBuyViewModel>().ReverseMap();
        }
    }
}