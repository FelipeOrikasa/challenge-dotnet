using AutoMapper;
using Mottu.Api.DTOs.FilialDtos;
using Mottu.Api.DTOs.LocalizacaoDtos;
using Mottu.Api.DTOs.MotoDtos;
using Mottu.Api.DTOs.PatioDtos;
using Mottu.Api.DTOs.SensorDtos;
using Mottu.Api.Models;

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
                .ForMember(dest => dest.NomeFilial, opt => opt.MapFrom(src => src.Filial.NomeFilial));
            CreateMap<CreatePatioDto, Patio>();
            CreateMap<UpdatePatioDto, Patio>();

            // --- Mapeamentos para Moto ---
            CreateMap<Moto, ReadMotoDto>()
                .ForMember(dest => dest.NomePatio, opt => opt.MapFrom(src => src.Patio.NomePatio));
            CreateMap<CreateMotoDto, Moto>();
            CreateMap<UpdateMotoDto, Moto>();

            // --- Mapeamentos para Sensor ---
            CreateMap<Sensor, ReadSensorDto>()
                .ForMember(dest => dest.NomePatio, opt => opt.MapFrom(src => src.Patio.NomePatio));
            CreateMap<CreateSensorDto, Sensor>();
            CreateMap<UpdateSensorDto, Sensor>();

            // --- Mapeamentos para Localizacao ---
            CreateMap<Localizacao, ReadLocalizacaoDto>()
                .ForMember(dest => dest.PlacaMoto, opt => opt.MapFrom(src => src.Moto.Placa))
                .ForMember(dest => dest.DescricaoSensor, opt => opt.MapFrom(src => src.Sensor.Descricao))
                .ForMember(dest => dest.NomePatio, opt => opt.MapFrom(src => src.Sensor.Patio.NomePatio));
            CreateMap<CreateLocalizacaoDto, Localizacao>();
        }
    }
}