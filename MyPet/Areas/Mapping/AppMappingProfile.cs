using AutoMapper;
using MyPet.Models;
using MyPet.ViewModels;
using System;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<MainProductModel, ProductViewModel>();
        CreateMap<ProductViewModel, MainProductModel>();

        CreateMap<MainProductModel?, ProductViewModel?>();
        CreateMap<ProductViewModel?, MainProductModel?>();

        CreateMap<ProductDetailedViewModel?, MainProductModel?>();
        CreateMap<MainProductModel?, ProductDetailedViewModel?>();

        CreateMap<ProductDetailedViewModel, MainProductModel>();
        CreateMap<MainProductModel, ProductDetailedViewModel>();

        CreateMap<CreateProductViewModel, MainProductModel>();
        CreateMap<MainProductModel, CreateProductViewModel>();
        CreateMap<CreateProductViewModel?, MainProductModel?>();
        CreateMap<MainProductModel?, CreateProductViewModel?>();

        //CreateMap<ProductViewModel, ProductModel>()
        //    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

    }
}