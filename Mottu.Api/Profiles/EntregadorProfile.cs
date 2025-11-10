using AutoMapper;
using Mottu.Api.Models.DTOs.Request; // Namespace correto
using Mottu.Api.Models.DTOs.Response; // Namespace correto
using Mottu.Api.Models.Entities;

namespace Mottu.Api.Mappers
{
    /// <summary>
    /// Perfil de mapeamento do AutoMapper para a entidade Entregador.
    /// </summary>
    public class EntregadorProfile : Profile
    {
        public EntregadorProfile()
        {
            // Mapeia do DTO de Requisição para a Entidade (Criação)
            // CORRIGIDO: Usando EntregadorRequest (sem Dto no nome)
            CreateMap<EntregadorRequest, Entregador>();

            // Mapeia do DTO de Requisição para a Entidade (Atualização)
            // CORRIGIDO: Usando EntregadorUpdateRequest
            CreateMap<EntregadorUpdateRequest, Entregador>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); 

            // Mapeia da Entidade para o DTO de Resposta
            // CORRIGIDO: Usando EntregadorResponse (sem Dto no nome)
            CreateMap<Entregador, EntregadorResponse>();
        }
    }
}