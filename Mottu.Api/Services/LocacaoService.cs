using AutoMapper;
using Mottu.Api.Models.DTOs.Request;
using Mottu.Api.Models.DTOs.Response;
using Mottu.Api.Models.Entities;
using Mottu.Api.Models.Enums;
using Mottu.Api.Repositories.Interfaces;
using Mottu.Api.Services.Interfaces;
using Mottu.Api.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mottu.Api.Services.Implementations
{
    public class LocacaoService : ILocacaoService
    {
        private readonly ILocacaoRepository _locacaoRepository;
        private readonly IEntregadorRepository _entregadorRepository;
        private readonly IMotoRepository _motoRepository;
        private readonly IMapper _mapper;

        public LocacaoService(
            ILocacaoRepository locacaoRepository,
            IEntregadorRepository entregadorRepository,
            IMotoRepository motoRepository,
            IMapper mapper)
        {
            _locacaoRepository = locacaoRepository;
            _entregadorRepository = entregadorRepository;
            _motoRepository = motoRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<LocacaoResponseDto>> RentMotoAsync(LocacaoRequestDto dto)
        {
            // Validação: Entregador existe
            var entregador = await _entregadorRepository.GetByIdAsync(dto.EntregadorId);
            if (entregador == null)
            {
                return new ApiResponse<LocacaoResponseDto>(404, "Entregador não encontrado.");
            }

            // Validação: Moto existe será feita através do LocacaoRepository.GetActiveByMotoIdAsync

            // Validação: Entregador não tem locação ativa
            var locacaoAtivaEntregador = await _locacaoRepository.GetActiveByEntregadorIdAsync(dto.EntregadorId);
            if (locacaoAtivaEntregador != null)
            {
                return new ApiResponse<LocacaoResponseDto>(409, "Entregador já possui uma locação ativa.");
            }

            // Validação: Moto não está alugada
            var locacaoAtivaMoto = await _locacaoRepository.GetActiveByMotoIdAsync(dto.MotoId);
            if (locacaoAtivaMoto != null)
            {
                return new ApiResponse<LocacaoResponseDto>(409, "Moto já está alugada.");
            }

            // Validação: Plano de dias válido
            var planosValidos = new[] { 7, 15, 30, 45, 50 };
            if (!planosValidos.Contains(dto.PlanoDias))
            {
                return new ApiResponse<LocacaoResponseDto>(400, "Plano de dias inválido. Deve ser 7, 15, 30, 45 ou 50 dias.");
            }

            // Criar locação
            var locacao = _mapper.Map<Locacao>(dto);
            locacao.Id = Guid.NewGuid();
            locacao.DataInicio = DateTime.UtcNow.Date;
            locacao.DataTerminoPrevista = locacao.DataInicio.AddDays(dto.PlanoDias);
            locacao.DiasContratados = dto.PlanoDias;
            locacao.CustoDiarioContratado = 50.00m; // Valor fixo conforme regra de negócio
            locacao.CustoTotalPrevisto = locacao.CustoDiarioContratado * dto.PlanoDias;
            locacao.CustoFinal = 0;
            locacao.Status = StatusLocacao.Ativa;

            await _locacaoRepository.AddAsync(locacao);

            var response = _mapper.Map<LocacaoResponseDto>(locacao);
            return new ApiResponse<LocacaoResponseDto>(201, response);
        }

        public async Task<ApiResponse<LocacaoResponseDto>> DevolucaoMotoAsync(Guid locacaoId, DateTime dataDevolucao)
        {
            var locacao = await _locacaoRepository.GetByIdAsync(locacaoId);
            if (locacao == null)
            {
                return new ApiResponse<LocacaoResponseDto>(404, "Locação não encontrada.");
            }

            if (locacao.DataTerminoEfetiva.HasValue)
            {
                return new ApiResponse<LocacaoResponseDto>(400, "Locação já foi finalizada.");
            }

            locacao.DataTerminoEfetiva = dataDevolucao.Date;
            
            // Calcular custo final
            var diasUsados = (dataDevolucao.Date - locacao.DataInicio.Date).Days;
            var diasContratados = locacao.DiasContratados;

            if (diasUsados <= diasContratados)
            {
                // Devolução no prazo ou antecipada
                locacao.CustoFinal = locacao.CustoDiarioContratado * diasUsados;
                locacao.Status = diasUsados < diasContratados 
                    ? StatusLocacao.FinalizadaAntecipada 
                    : StatusLocacao.FinalizadaNoPrazo;
            }
            else
            {
                // Devolução com atraso
                var diasExtras = diasUsados - diasContratados;
                var multaPorDia = locacao.CustoDiarioContratado * 0.5m; // 50% de multa por dia extra
                locacao.CustoFinal = locacao.CustoTotalPrevisto + (multaPorDia * diasExtras);
                locacao.Status = StatusLocacao.FinalizadaAtrasada;
            }

            await _locacaoRepository.UpdateAsync(locacao);

            var response = _mapper.Map<LocacaoResponseDto>(locacao);
            return new ApiResponse<LocacaoResponseDto>(200, response);
        }

        public async Task<ApiResponse<LocacaoResponseDto?>> GetLocacaoByIdAsync(Guid id)
        {
            var locacao = await _locacaoRepository.GetByIdAsync(id);
            if (locacao == null)
            {
                return new ApiResponse<LocacaoResponseDto?>(404, "Locação não encontrada.");
            }

            var response = _mapper.Map<LocacaoResponseDto>(locacao);
            return new ApiResponse<LocacaoResponseDto?>(200, response);
        }

        public async Task<ApiResponse<IEnumerable<LocacaoResponseDto>>> GetAllLocacoesAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("LocacaoService.GetAllLocacoesAsync: Iniciando consulta");
                Console.WriteLine("LocacaoService.GetAllLocacoesAsync: Iniciando consulta");
                
                var locacoes = await _locacaoRepository.GetAllAsync();
                var locacoesList = locacoes.ToList();
                
                System.Diagnostics.Debug.WriteLine($"LocacaoService.GetAllLocacoesAsync: Encontradas {locacoesList.Count} locacoes do banco");
                Console.WriteLine($"LocacaoService.GetAllLocacoesAsync: Encontradas {locacoesList.Count} locacoes do banco");
                
                if (locacoesList.Count > 0)
                {
                    var primeira = locacoesList.First();
                    System.Diagnostics.Debug.WriteLine($"Primeira locacao - Id: {primeira.Id}, EntregadorId: {primeira.EntregadorId}, MotoId: {primeira.MotoId}");
                    Console.WriteLine($"Primeira locacao - Id: {primeira.Id}, EntregadorId: {primeira.EntregadorId}, MotoId: {primeira.MotoId}");
                }
                
                var response = _mapper.Map<IEnumerable<LocacaoResponseDto>>(locacoesList);
                var responseList = response.ToList();
                
                System.Diagnostics.Debug.WriteLine($"LocacaoService.GetAllLocacoesAsync: Após mapeamento, {responseList.Count} locacoes no response");
                Console.WriteLine($"LocacaoService.GetAllLocacoesAsync: Após mapeamento, {responseList.Count} locacoes no response");
                
                return new ApiResponse<IEnumerable<LocacaoResponseDto>>(200, responseList);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LocacaoService.GetAllLocacoesAsync: ERRO - {ex.Message}");
                Console.WriteLine($"LocacaoService.GetAllLocacoesAsync: ERRO - {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<ApiResponse<LocacaoResponseDto?>> GetActiveLocacaoByEntregadorIdAsync(Guid entregadorId)
        {
            var locacao = await _locacaoRepository.GetActiveByEntregadorIdAsync(entregadorId);
            if (locacao == null)
            {
                return new ApiResponse<LocacaoResponseDto?>(404, "Nenhuma locação ativa encontrada para este entregador.");
            }

            var response = _mapper.Map<LocacaoResponseDto>(locacao);
            return new ApiResponse<LocacaoResponseDto?>(200, response);
        }
    }
}
