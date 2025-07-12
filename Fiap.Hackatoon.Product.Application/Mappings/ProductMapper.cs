using AutoMapper;
using DTO = Fiap.Hackatoon.Product.Application.DataTransferObjects;
using DO = Fiap.Hackatoon.Product.Domain.Entities;
using MSG = Fiap.Hackatoon.Product.Application.DataTransferObjects.MessageBrokers;
using VIEW = Fiap.Hackatoon.Product.Domain.Views.ElasticSearch;
using DTOE = Fiap.Hackatoon.Product.Application.DataTransferObjects.ElasticSearch;

namespace Fiap.Hackatoon.Product.Application.Mappings;

public class ProductMapper : Profile
{
    public ProductMapper()
    {
        CreateMap<DTO.Product, DO.Product>()
            .ForMember(dest => dest.Id, opt =>
            {
                opt.Condition(src => src.Id.HasValue == false || src.Id.Value == default);
                opt.UseDestinationValue();
            })
            .ForMember(dest => dest.Type, opt => opt.Ignore())
            .ReverseMap()
            ;

        CreateMap<DTO.ProductType, DO.ProductType>()
            .ForMember(dest => dest.Id, opt =>
            {
                opt.Condition(src => src.Id.HasValue == false || src.Id.Value == default);
                opt.UseDestinationValue();
            })
            .ReverseMap();

        CreateMap<DTO.Product, MSG.Product>()
            .ForMember(dest => dest.Id, opt =>
            {
                opt.Condition(src => src.Id.HasValue == false || src.Id.Value == default);
                opt.UseDestinationValue();
            })
            .ReverseMap();

        CreateMap<DO.Product, VIEW.ProductByType>()
            .ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.Type.Id))
            .ForMember(dest => dest.TypeCode, opt => opt.MapFrom(src => src.Type.Code))
            .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type.Name))
            .ForMember(dest => dest.TypeDescription, opt => opt.MapFrom(src => src.Type.Description));

        CreateMap<VIEW.ProductByType, DTOE.ProductByType>();
        CreateMap<DTOE.ProductByType, DTO.Product>()
            .ForMember(dest => dest.Id, opt =>
            {
                opt.Condition(src => src.Id.HasValue == false || src.Id.Value == default);
                opt.UseDestinationValue();
            })
            .ForMember(dest => dest.Type, opt => opt.Ignore())
            .ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.TypeId))
            .ReverseMap();
    }
}