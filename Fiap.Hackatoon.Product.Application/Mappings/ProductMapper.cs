using AutoMapper;
using DTO = Fiap.Hackatoon.Product.Application.DataTransferObjects;
using DO = Fiap.Hackatoon.Product.Domain.Entities;
using MSG = Fiap.Hackatoon.Product.Application.DataTransferObjects.MessageBrokers;

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
            .ForMember(dest => dest.OrderProducts, opt => opt.Ignore())
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
    }
}