using AutoMapper;
using bg.crm.integration.application.dtos.models.productos.tarjetas;
using bg.crm.integration.domain.entities.producto.tarjetas;

namespace bg.crm.integration.infrastructure.MapperConfig
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TarjetaCreditoEntity, TarjetaCreditoDto>()
                .ForMember(dest => dest.NumeroEnmascarado, opt => opt.MapFrom(src => src.NumeroEnmascarado))
                .ForMember(dest => dest.NumeroTarjeta, opt => opt.MapFrom(src => src.NumeroTarjeta))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Bin, opt => opt.MapFrom(src => src.Bin))
                .ForMember(dest => dest.FechaExpedicion, opt => opt.MapFrom(src => src.FechaExpedicion))
                .ForMember(dest => dest.FechaExpiracion, opt => opt.MapFrom(src => src.FechaExpiracion))
                .ForMember(dest => dest.SecureCode, opt => opt.MapFrom(src => src.SecureCode));
        }
    }
}