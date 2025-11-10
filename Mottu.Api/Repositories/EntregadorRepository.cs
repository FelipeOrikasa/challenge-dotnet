using Microsoft.EntityFrameworkCore;
using Mottu.Api.Data;
using Mottu.Api.Models.Entities;
using Mottu.Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mottu.Api.Repositories
{
    /// <summary>
    /// Repositório de persistência de dados do Entregador, usando Entity Framework Core.
    /// </summary>
    public class EntregadorRepository : IEntregadorRepository
    {
        private readonly AppDbContext _context;

        public EntregadorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Entregador?> GetByIdAsync(Guid id)
        {
            return await _context.Entregadores.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Entregador>> GetAllAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("EntregadorRepository.GetAllAsync: Iniciando consulta");
                Console.WriteLine("EntregadorRepository.GetAllAsync: Iniciando consulta");
                
                var entregadores = await _context.Entregadores.ToListAsync();
                
                System.Diagnostics.Debug.WriteLine($"EntregadorRepository.GetAllAsync: Encontrados {entregadores.Count} entregadores");
                Console.WriteLine($"EntregadorRepository.GetAllAsync: Encontrados {entregadores.Count} entregadores");
                
                if (entregadores.Count > 0)
                {
                    var primeiro = entregadores.First();
                    System.Diagnostics.Debug.WriteLine($"Primeiro entregador - Id: {primeiro.Id}, Nome: {primeiro.Nome}, CNPJ: {primeiro.CNPJ}");
                    Console.WriteLine($"Primeiro entregador - Id: {primeiro.Id}, Nome: {primeiro.Nome}, CNPJ: {primeiro.CNPJ}");
                }
                
                return entregadores;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EntregadorRepository.GetAllAsync: ERRO - {ex.Message}");
                Console.WriteLine($"EntregadorRepository.GetAllAsync: ERRO - {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task AddAsync(Entregador entregador)
        {
            await _context.Entregadores.AddAsync(entregador);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Entregador entregador)
        {
            _context.Entregadores.Update(entregador);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entregador = await GetByIdAsync(id);
            if (entregador != null)
            {
                _context.Entregadores.Remove(entregador);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Entregador?> GetByCNPJAsync(string cnpj)
        {
            return await _context.Entregadores.FirstOrDefaultAsync(e => e.CNPJ == cnpj);
        }

        public async Task<Entregador?> GetByCNHAsync(string cnh)
        {
            return await _context.Entregadores.FirstOrDefaultAsync(e => e.CNH == cnh);
        }
    }
}

