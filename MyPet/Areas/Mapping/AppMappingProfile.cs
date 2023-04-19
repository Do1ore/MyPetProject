using AutoMapper;
using MyPet.Models;
using MyPet.ViewModels;
using MyPet.ViewModels.DTOs.News;
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

        CreateMap<CartProduct, CartProductViewModel>();
        CreateMap<CartProductViewModel, CartProduct>();

        CreateMap<CartProduct?, CartProductViewModel?>();
        CreateMap<CartProductViewModel?, CartProduct?>();

        CreateMap<NewsApiSettingsModel, NewsSettingsViewModel>();
        CreateMap<NewsSettingsViewModel, NewsApiSettingsModel>();

        CreateMap<NewsApiSettingsModel?, NewsSettingsViewModel?>();
        CreateMap<NewsSettingsViewModel?, NewsApiSettingsModel?>();


        CreateMap<MainProductModel, ProductAndQuantityViewModel>();
        CreateMap<ProductAndQuantityViewModel, MainProductModel>();

        CreateMap<MainProductModel?, ProductAndQuantityViewModel?>();
        CreateMap<ProductAndQuantityViewModel?, MainProductModel?>();

        CreateMap<ProductReview, ProductReviewViewModel>();
        CreateMap<ProductReviewViewModel, ProductReview>();

        //CreateMap<ProductViewModel, ProductModel>()
        //    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

    }
}
