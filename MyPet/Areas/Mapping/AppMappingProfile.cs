using AutoMapper;
using MyPet.ViewModels;
using System;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        //CreateMap<ProductModel, ProductViewModel>()
        //     .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
        
        //CreateMap<ProductViewModel, ProductModel>()
        //    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

    }
}