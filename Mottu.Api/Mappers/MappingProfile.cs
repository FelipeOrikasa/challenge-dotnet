using AutoMapper;
using Mottu.Api.DTOs.FilialDtos;
using Mottu.Api.DTOs.LocalizacaoDtos;
using Mottu.Api.DTOs.MotoDtos;
using Mottu.Api.DTOs.PatioDtos;
using Mottu.Api.DTOs.SensorDtos;
using Mottu.Api.Models.Entities;

namespace Mottu.Api.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // --- Mapeamentos para Filial ---
            CreateMap<Filial, ReadFilialDto>();
            CreateMap<CreateFilialDto, Filial>();
            CreateMap<UpdateFilialDto, Filial>();

            // --- Mapeamentos para Patio ---
            CreateMap<Patio, ReadPatioDto>()
                // AGORA FUNCIONA: Filial tem a propriedade NomeFilial.
                .ForMember(dest => dest.NomeFilial, opt => opt.MapFrom(src => src.Filial.NomeFilial));
            CreateMap<CreatePatioDto, Patio>();
            CreateMap<UpdatePatioDto, Patio>();

            // --- Mapeamentos para Moto ---
            CreateMap<Moto, ReadMotoDto>()
                // Mapeia Id para MotoId
                .ForMember(dest => dest.MotoId, opt => opt.MapFrom(src => src.Id))
                // AGORA FUNCIONA: Moto tem a propriedade de navegação Patio.
                .ForMember(dest => dest.NomePatio, opt => opt.MapFrom(src => src.Patio != null ? src.Patio.NomePatio : string.Empty));
            // NOTA: Mapeamento Moto -> MotoResponse está no MotoProfile para evitar conflito
            CreateMap<CreateMotoDto, Moto>();
            CreateMap<UpdateMotoDto, Moto>();

            // --- Mapeamentos para Sensor ---
            CreateMap<Sensor, ReadSensorDto>()
                .ForMember(dest => dest.NomePatio, opt => opt.MapFrom(src => src.Patio.NomePatio));
            CreateMap<CreateSensorDto, Sensor>();
            CreateMap<UpdateSensorDto, Sensor>();

            // --- Mapeamentos para Localizacao ---
            CreateMap<Localizacao, ReadLocalizacaoDto>()
                // Nota: Localizacao não tem relação direta com Moto no modelo atual
                // PlacaMoto pode ser obtida através do Sensor -> Patio -> Motos, mas isso é complexo
                // Por enquanto, deixamos vazio ou obtemos de outra forma
                .ForMember(dest => dest.PlacaMoto, opt => opt.Ignore())
                .ForMember(dest => dest.DescricaoSensor, opt => opt.MapFrom(src => src.Sensor != null ? src.Sensor.Descricao : string.Empty))
                .ForMember(dest => dest.NomePatio, opt => opt.MapFrom(src => src.Sensor != null && src.Sensor.Patio != null ? src.Sensor.Patio.NomePatio : string.Empty));
            CreateMap<CreateLocalizacaoDto, Localizacao>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Será gerado no serviço
                .ForMember(dest => dest.Timestamp, opt => opt.Ignore()) // Será gerado no serviço
                .ForMember(dest => dest.Latitude, opt => opt.Ignore()) // Será gerado no serviço
                .ForMember(dest => dest.Longitude, opt => opt.Ignore()); // Será gerado no serviço
                // MotoId do DTO não é mapeado - é usado apenas para validação no serviço
        }
    }
}