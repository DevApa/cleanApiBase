using AutoMapper;
using bg.crm.integration.application.dtos.models.productos.creditos;
using bg.crm.integration.domain.entities.producto.creditos;

namespace bg.crm.integration.infrastructure.MapperConfig
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreditoResponse, CreditoResponseDto>()
            .ForMember(d => d.NumeroOperacion, o => o.MapFrom(s => s.NumeroOperacionn))
            .ForMember(d => d.TipoCredito, o => o.MapFrom(s => s.TipoCrediton))
            .ForMember(d => d.TipoMoneda, o => o.MapFrom(s => s.TipoMonedan))
            .ForMember(d => d.TipoTasa, o => o.MapFrom(s => s.TipoTasan))
            .ForMember(d => d.TasaInteres, o => o.MapFrom(s => s.TasaInteresn))
            .ForMember(d => d.Monto, o => o.MapFrom(s => s.Monton))
            .ForMember(d => d.FechaDesembolso, o => o.MapFrom(s => s.FechaDesembolson))
            .ForMember(d => d.FechaVencimiento, o => o.MapFrom(s => s.FechaVencimienton))
            .ForMember(d => d.FechaUltimoPago, o => o.MapFrom(s => s.FechaUltimoPagon))
            .ForMember(d => d.FechaUltimoEstado, o => o.MapFrom(s => s.FechaUltimoEstadon))
            .ForMember(d => d.Canal, o => o.MapFrom(s => s.Canaln))
            .ForMember(d => d.Estado, o => o.MapFrom(s => s.Estadon))
            .ForMember(d => d.PlazoDias, o => o.MapFrom(s => s.PlazoDiasn))
            .ForMember(d => d.PlazoMeses, o => o.MapFrom(s => s.PlazoMesesn))
            .ForMember(d => d.TasaInicial, o => o.MapFrom(s => s.TasaInicialn))
            .ForMember(d => d.TasaEfectiva, o => o.MapFrom(s => s.TasaEfectivan))
            .ForMember(d => d.Cesantia, o => o.MapFrom(s => s.Cesantian))
            .ForMember(d => d.FactorReajuste, o => o.MapFrom(s => s.FactorReajusten))
            .ForMember(d => d.Comision, o => o.MapFrom(s => s.Comisionn))
            .ForMember(d => d.SeguroDesgravamen, o => o.MapFrom(s => s.SeguroDesgravamenn))
            .ForMember(d => d.TipoAmortizacion, o => o.MapFrom(s => s.TipoAmortizacionn))
            .ForMember(d => d.CuentaDesembolso, o => o.MapFrom(s => s.CuentaDesembolson))
            .ForMember(d => d.DestinoFondos, o => o.MapFrom(s => s.DestinoFondosn))
            .ForMember(d => d.TipoGarantia, o => o.MapFrom(s => s.TipoGarantian))
            .ForMember(d => d.Aprobaciones, o => o.MapFrom(s => s.Aprobacionesn));


        }
    }
}