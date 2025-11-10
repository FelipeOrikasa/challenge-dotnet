using AutoMapper;
using Mottu.Api.Models.DTOs.Request;
using Mottu.Api.Models.DTOs.Response;
using Mottu.Api.Models.Entities;

namespace Mottu.Api.Mappers
{
    /// <summary>
    /// Perfil de mapeamento do AutoMapper para a entidade Moto.
    /// </summary>
    public class MotoProfile : Profile
    {
        public MotoProfile()
        {
            // Mapeia do DTO de Requisição para a Entidade (Criação)
            CreateMap<MotoRequest, Moto>();

            // Mapeia do DTO de Atualização para a Entidade (Atualização de Placa)
            // OBS: O automapper só será usado para copiar a placa.
            CreateMap<MotoUpdateRequest, Moto>();

            // Mapeia da Entidade para o DTO de Resposta (Leitura)
            CreateMap<Moto, MotoResponse>()
                .ForMember(dest => dest.Modelo, opt => opt.MapFrom(src => src.Modelo ?? "Modelo não informado"))
                .ForMember(dest => dest.Placa, opt => opt.MapFrom(src => src.Placa ?? ""))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Ano, opt => opt.MapFrom(src => src.Ano));
        }
    }
}