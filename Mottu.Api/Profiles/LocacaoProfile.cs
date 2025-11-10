using AutoMapper;
using Mottu.Api.Models.DTOs.Request;
using Mottu.Api.Models.DTOs.Response;
using Mottu.Api.Models.Entities;
using System;

namespace Mottu.Api.Mappers
{
    /// <summary>
    /// Perfil de mapeamento do AutoMapper para a entidade Locacao.
    /// </summary>
    public class LocacaoProfile : Profile
    {
        public LocacaoProfile()
        {
            // Request DTO para Entidade (Criação)
            CreateMap<LocacaoRequestDto, Locacao>();

            // Entidade para Response DTO: calcula o status no mapeamento
            CreateMap<Locacao, LocacaoResponseDto>() 
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => 
                    src.DataTerminoEfetiva.HasValue 
                        ? "Concluída" 
                        : (src.DataTerminoPrevista.Date < DateTime.Today.Date // Comparando apenas as datas
                            ? "Atrasada" 
                            : "Ativa")));
        }
    }
}