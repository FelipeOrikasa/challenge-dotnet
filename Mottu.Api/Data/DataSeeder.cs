using Microsoft.EntityFrameworkCore;
using Mottu.Api.Models.Entities;
using Mottu.Api.Models.Enums;
using System;
using System.Linq;

namespace Mottu.Api.Data
{
    /// <summary>
    /// Classe responsável por popular o banco de dados com dados iniciais.
    /// </summary>
    public static class DataSeeder
    {
        /// <summary>
        /// Popula o banco de dados com dados iniciais se estiver vazio.
        /// </summary>
        /// <param name="context">O contexto do banco de dados.</param>
        /// <param name="forceSeed">Se true, força a população mesmo se já existirem dados.</param>
        public static async Task SeedAsync(AppDbContext context, bool forceSeed = false)
        {
            // Verifica se já existem dados (usando Count para evitar problemas com booleanos no Oracle)
            // Verifica múltiplas tabelas para garantir que o banco está realmente vazio
            var filiaisCount = await context.Filiais.CountAsync();
            var patiosCount = await context.Patios.CountAsync();
            var motosCount = await context.Motos.CountAsync();
            var entregadoresCount = await context.Entregadores.CountAsync();
            
            // Atualiza dados antigos que podem estar incompatíveis (ex: Motos sem Modelo)
            await UpdateExistingDataAsync(context);
            
            // Verifica se precisa popular cada tabela individualmente
            var precisaPopularFiliais = forceSeed || filiaisCount == 0;
            var precisaPopularPatios = forceSeed || patiosCount == 0;
            var precisaPopularMotos = forceSeed || motosCount == 0;
            var precisaPopularEntregadores = forceSeed || entregadoresCount == 0;
            var locacoesCount = await context.Locacoes.CountAsync();
            var precisaPopularLocacoes = forceSeed || locacoesCount == 0;
            var sensoresCount = await context.Sensores.CountAsync();
            var precisaPopularSensores = forceSeed || sensoresCount == 0;
            
            // Se não precisa popular nada e não está forçando, sai
            if (!forceSeed && !precisaPopularFiliais && !precisaPopularPatios && !precisaPopularMotos && !precisaPopularEntregadores && !precisaPopularLocacoes && !precisaPopularSensores)
            {
                Console.WriteLine($"DataSeeder: Banco já populado. Filiais: {filiaisCount}, Patios: {patiosCount}, Motos: {motosCount}, Entregadores: {entregadoresCount}, Locacoes: {locacoesCount}, Sensores: {sensoresCount}");
                Console.WriteLine("DataSeeder: Para forçar a população, defina FORCE_SEED=true ou limpe as tabelas manualmente.");
                return;
            }
            
            if (forceSeed)
            {
                Console.WriteLine("DataSeeder: Modo FORCE_SEED ativado - populando mesmo com dados existentes.");
            }
            
            System.Diagnostics.Debug.WriteLine("DataSeeder: Iniciando população do banco de dados...");
            Console.WriteLine("DataSeeder: Iniciando população do banco de dados...");

            // 1. Criar Filiais (se necessário)
            Filial filial1 = null!, filial2 = null!, filial3 = null!;
            if (precisaPopularFiliais)
            {
                filial1 = new Filial
                {
                    Id = 1,
                    NomeFilial = "Filial São Paulo - Centro",
                    Endereco = "Av. Paulista, 1000 - São Paulo/SP"
                };

                filial2 = new Filial
                {
                    Id = 2,
                    NomeFilial = "Filial Rio de Janeiro - Copacabana",
                    Endereco = "Av. Atlântica, 2000 - Rio de Janeiro/RJ"
                };

                filial3 = new Filial
                {
                    Id = 3,
                    NomeFilial = "Filial Belo Horizonte - Centro",
                    Endereco = "Av. Afonso Pena, 3000 - Belo Horizonte/MG"
                };

                context.Filiais.AddRange(filial1, filial2, filial3);
                await context.SaveChangesAsync();
                Console.WriteLine("DataSeeder: Filiais criadas.");
            }
            else
            {
                // Busca filiais existentes para usar nas relações
                filial1 = await context.Filiais.FirstOrDefaultAsync(f => f.Id == 1) ?? 
                    await context.Filiais.FirstAsync();
                filial2 = await context.Filiais.FirstOrDefaultAsync(f => f.Id == 2) ?? filial1;
                filial3 = await context.Filiais.FirstOrDefaultAsync(f => f.Id == 3) ?? filial1;
            }

            // 2. Criar Pátios (se necessário)
            Patio patio1 = null!, patio2 = null!, patio3 = null!, patio4 = null!;
            if (precisaPopularPatios)
            {
                patio1 = new Patio
                {
                    Id = 1,
                    NomePatio = "Pátio A - Vistoria e Manutenção",
                    CapacidadeMaxima = 50,
                    FilialId = 1,
                    Filial = filial1
            };

                patio2 = new Patio
                {
                    Id = 2,
                    NomePatio = "Pátio B - Estacionamento",
                    CapacidadeMaxima = 100,
                    FilialId = 1,
                    Filial = filial1
            };

                patio3 = new Patio
                {
                    Id = 3,
                    NomePatio = "Pátio C - Entrada Principal",
                    CapacidadeMaxima = 30,
                    FilialId = 2,
                    Filial = filial2
            };

                patio4 = new Patio
                {
                    Id = 4,
                    NomePatio = "Pátio D - Saída de Entrega",
                    CapacidadeMaxima = 40,
                    FilialId = 3,
                    Filial = filial3
            };

                context.Patios.AddRange(patio1, patio2, patio3, patio4);
                await context.SaveChangesAsync();
                Console.WriteLine("DataSeeder: Patios criados.");
            }
            else
            {
                // Busca patios existentes para usar nas relações
                patio1 = await context.Patios.FirstOrDefaultAsync(p => p.Id == 1) ?? 
                    await context.Patios.FirstAsync();
                patio2 = await context.Patios.FirstOrDefaultAsync(p => p.Id == 2) ?? patio1;
                patio3 = await context.Patios.FirstOrDefaultAsync(p => p.Id == 3) ?? patio1;
                patio4 = await context.Patios.FirstOrDefaultAsync(p => p.Id == 4) ?? patio1;
            }

            // 3. Criar Sensores (se necessário e se houver Patios)
            if (precisaPopularSensores && patio1 != null)
            {
                var sensor1 = new Sensor
                {
                    Id = 1,
                    Descricao = "Sensor de RFID - Portão de Entrada",
                    Ativo = true,
                    PatioId = 1,
                    Patio = patio1
            };

                var sensor2 = new Sensor
                {
                    Id = 2,
                    Descricao = "Sensor de RFID - Portão de Saída",
                    Ativo = true,
                    PatioId = 1,
                    Patio = patio1
            };

                var sensor3 = new Sensor
                {
                    Id = 3,
                    Descricao = "Sensor GPS - Área de Estacionamento",
                    Ativo = true,
                    PatioId = 2,
                    Patio = patio2
            };

                var sensor4 = new Sensor
                {
                    Id = 4,
                    Descricao = "Sensor de RFID - Entrada Principal",
                    Ativo = true,
                    PatioId = 3,
                    Patio = patio3
            };

                var sensor5 = new Sensor
                {
                    Id = 5,
                    Descricao = "Sensor GPS - Saída de Entrega",
                    Ativo = true,
                    PatioId = 4,
                    Patio = patio4
            };

                context.Sensores.AddRange(sensor1, sensor2, sensor3, sensor4, sensor5);
                await context.SaveChangesAsync();
                Console.WriteLine("DataSeeder: Sensores criados.");
            }

            // 4. Criar Motos (se necessário)
            Moto moto1 = null!, moto2 = null!, moto3 = null!, moto4 = null!, moto5 = null!, moto6 = null!;
            if (precisaPopularMotos)
            {
                moto1 = new Moto
                {
                    Id = Guid.NewGuid(),
                    Placa = "ABC1234",
                    Modelo = "Honda CB 300F Twister",
                    Ano = 2024,
                    PatioId = 1,
                    Patio = patio1
            };

                moto2 = new Moto
                {
                    Id = Guid.NewGuid(),
                    Placa = "DEF5678",
                    Modelo = "Yamaha MT-03",
                    Ano = 2023,
                    PatioId = 1,
                    Patio = patio1
            };

                moto3 = new Moto
                {
                    Id = Guid.NewGuid(),
                    Placa = "GHI9012",
                    Modelo = "Honda PCX 150",
                    Ano = 2024,
                    PatioId = 2,
                    Patio = patio2
            };

                moto4 = new Moto
                {
                    Id = Guid.NewGuid(),
                    Placa = "JKL3456",
                    Modelo = "Yamaha NMAX",
                    Ano = 2023,
                    PatioId = 2,
                    Patio = patio2
            };

                moto5 = new Moto
                {
                    Id = Guid.NewGuid(),
                    Placa = "MNO7890",
                    Modelo = "Honda CB 600F Hornet",
                    Ano = 2024,
                    PatioId = 3,
                    Patio = patio3
            };

                moto6 = new Moto
                {
                    Id = Guid.NewGuid(),
                    Placa = "PQR2345",
                    Modelo = "Kawasaki Ninja 300",
                    Ano = 2023,
                    PatioId = 4,
                    Patio = patio4
            };

                context.Motos.AddRange(moto1, moto2, moto3, moto4, moto5, moto6);
                await context.SaveChangesAsync();
                Console.WriteLine("DataSeeder: Motos criadas.");
            }
            else
            {
                // Busca motos existentes para usar nas relações
                var motosExistentes = await context.Motos.Take(6).ToListAsync();
                if (motosExistentes.Count > 0) moto1 = motosExistentes[0];
                if (motosExistentes.Count > 1) moto2 = motosExistentes[1];
                if (motosExistentes.Count > 2) moto3 = motosExistentes[2];
                if (motosExistentes.Count > 3) moto4 = motosExistentes[3];
                if (motosExistentes.Count > 4) moto5 = motosExistentes[4];
                if (motosExistentes.Count > 5) moto6 = motosExistentes[5];
                // Se não houver motos suficientes, usa a primeira para todas
                if (moto1 == null) moto1 = await context.Motos.FirstAsync();
                if (moto2 == null) moto2 = moto1;
                if (moto3 == null) moto3 = moto1;
                if (moto4 == null) moto4 = moto1;
                if (moto5 == null) moto5 = moto1;
                if (moto6 == null) moto6 = moto1;
            }

            // 5. Criar Entregadores (se necessário)
            Entregador entregador1 = null!, entregador2 = null!, entregador3 = null!, entregador4 = null!;
            if (precisaPopularEntregadores)
            {
                entregador1 = new Entregador
                {
                    Id = Guid.NewGuid(),
                    Nome = "João Silva",
                    CNPJ = "12.345.678/0001-90",
                    DataNascimento = new DateTime(1990, 5, 15),
                    CNH = "12345678901",
                    TipoCNH = "AB",
                    ImagemCNH = "/uploads/cnh/joao_silva.jpg"
            };

                entregador2 = new Entregador
                {
                    Id = Guid.NewGuid(),
                    Nome = "Maria Santos",
                    CNPJ = "98.765.432/0001-10",
                    DataNascimento = new DateTime(1988, 8, 22),
                    CNH = "98765432109",
                    TipoCNH = "A",
                    ImagemCNH = "/uploads/cnh/maria_santos.jpg"
            };

                entregador3 = new Entregador
                {
                    Id = Guid.NewGuid(),
                    Nome = "Pedro Oliveira",
                    CNPJ = "11.222.333/0001-44",
                    DataNascimento = new DateTime(1992, 3, 10),
                    CNH = "11223344556",
                    TipoCNH = "AB",
                    ImagemCNH = "/uploads/cnh/pedro_oliveira.jpg"
            };

                entregador4 = new Entregador
                {
                    Id = Guid.NewGuid(),
                    Nome = "Ana Costa",
                    CNPJ = "55.666.777/0001-88",
                    DataNascimento = new DateTime(1995, 11, 30),
                    CNH = "55667788990",
                    TipoCNH = "A",
                    ImagemCNH = "/uploads/cnh/ana_costa.jpg"
            };

                context.Entregadores.AddRange(entregador1, entregador2, entregador3, entregador4);
                await context.SaveChangesAsync();
                Console.WriteLine("DataSeeder: Entregadores criados.");
            }
            else
            {
                // Busca entregadores existentes para usar nas relações
                var entregadoresExistentes = await context.Entregadores.Take(4).ToListAsync();
                if (entregadoresExistentes.Count > 0) entregador1 = entregadoresExistentes[0];
                if (entregadoresExistentes.Count > 1) entregador2 = entregadoresExistentes[1];
                if (entregadoresExistentes.Count > 2) entregador3 = entregadoresExistentes[2];
                if (entregadoresExistentes.Count > 3) entregador4 = entregadoresExistentes[3];
                // Se não houver entregadores suficientes, usa o primeiro para todos
                if (entregador1 == null && entregadoresExistentes.Count > 0) entregador1 = entregadoresExistentes[0];
                if (entregador2 == null && entregador1 != null) entregador2 = entregador1;
                if (entregador3 == null && entregador1 != null) entregador3 = entregador1;
                if (entregador4 == null && entregador1 != null) entregador4 = entregador1;
            }

            // 6. Criar Locações (se necessário e se houver entregadores e motos)
            if (precisaPopularLocacoes && entregador1 != null && moto1 != null)
            {
                var locacao1 = new Locacao
                {
                    Id = Guid.NewGuid(),
                    EntregadorId = entregador1.Id,
                    MotoId = moto1.Id,
                    DataInicio = DateTime.Today.AddDays(-5),
                    DataTerminoPrevista = DateTime.Today.AddDays(2),
                    DataTerminoEfetiva = null,
                    DiasContratados = 7,
                    CustoDiarioContratado = 30m,
                    CustoTotalPrevisto = 210m,
                    CustoFinal = 0m,
                    Status = StatusLocacao.Ativa
            };

                var locacao2 = new Locacao
                {
                    Id = Guid.NewGuid(),
                    EntregadorId = (entregador2 ?? entregador1)!.Id,
                    MotoId = (moto3 ?? moto1)!.Id,
                    DataInicio = DateTime.Today.AddDays(-20),
                    DataTerminoPrevista = DateTime.Today.AddDays(10),
                    DataTerminoEfetiva = DateTime.Today.AddDays(-5), // Finalizada antecipadamente
                    DiasContratados = 30,
                    CustoDiarioContratado = 22m,
                    CustoTotalPrevisto = 660m,
                    CustoFinal = 550m, // Com multa por devolução antecipada
                    Status = StatusLocacao.FinalizadaAntecipada
            };

                var locacao3 = new Locacao
                {
                    Id = Guid.NewGuid(),
                    EntregadorId = (entregador3 ?? entregador1)!.Id,
                    MotoId = (moto5 ?? moto1)!.Id,
                    DataInicio = DateTime.Today.AddDays(-10),
                    DataTerminoPrevista = DateTime.Today.AddDays(-3),
                    DataTerminoEfetiva = DateTime.Today.AddDays(-1), // Finalizada com atraso
                    DiasContratados = 7,
                    CustoDiarioContratado = 30m,
                    CustoTotalPrevisto = 210m,
                    CustoFinal = 310m, // Com multa por atraso (2 dias x R$50)
                    Status = StatusLocacao.FinalizadaAtrasada
            };

                context.Locacoes.AddRange(locacao1, locacao2, locacao3);
                await context.SaveChangesAsync();
                Console.WriteLine("DataSeeder: Locacoes criadas.");
            }

            // 7. Criar Localizações (histórico de passagem de motos pelos sensores) - apenas se houver sensores
            var sensoresExistentes = await context.Sensores.Take(5).ToListAsync();
            if (sensoresExistentes.Count >= 3)
            {
                var localizacao1 = new Localizacao
                {
                    Id = Guid.NewGuid(),
                    SensorId = sensoresExistentes[0].Id,
                    Latitude = -23.5505m,
                    Longitude = -46.6333m,
                    Timestamp = DateTime.Now.AddHours(-2)
                };

                var localizacao2 = new Localizacao
                {
                    Id = Guid.NewGuid(),
                    SensorId = sensoresExistentes[1].Id,
                    Latitude = -23.5505m,
                    Longitude = -46.6333m,
                    Timestamp = DateTime.Now.AddHours(-1)
                };

                var localizacao3 = new Localizacao
                {
                    Id = Guid.NewGuid(),
                    SensorId = sensoresExistentes[2].Id,
                    Latitude = -23.5505m,
                    Longitude = -46.6333m,
                    Timestamp = DateTime.Now.AddMinutes(-30)
                };

                var localizacao4 = new Localizacao
                {
                    Id = Guid.NewGuid(),
                    SensorId = sensoresExistentes.Count > 3 ? sensoresExistentes[3].Id : sensoresExistentes[0].Id,
                    Latitude = -22.9068m,
                    Longitude = -43.1729m,
                    Timestamp = DateTime.Now.AddHours(-3)
                };

                context.Localizacoes.AddRange(localizacao1, localizacao2, localizacao3, localizacao4);
                await context.SaveChangesAsync();
                Console.WriteLine("DataSeeder: Localizacoes criadas.");
            }
            
            System.Diagnostics.Debug.WriteLine("DataSeeder: População concluída com sucesso!");
            Console.WriteLine("DataSeeder: População concluída com sucesso!");
            
            // Verificar se os dados foram salvos corretamente
            var filiaisFinais = await context.Filiais.CountAsync();
            var motosFinais = await context.Motos.CountAsync();
            Console.WriteLine($"DataSeeder: Verificação final - Filiais: {filiaisFinais}, Motos: {motosFinais}");
        }

        /// <summary>
        /// Atualiza dados antigos que podem estar incompatíveis com a nova estrutura.
        /// </summary>
        private static async Task UpdateExistingDataAsync(AppDbContext context)
        {
            try
            {
                Console.WriteLine("DataSeeder: Verificando dados antigos para atualização...");
                
                // Primeiro, tenta buscar todas as motos para ver se há problema com a consulta
                var todasMotos = await context.Motos
                    .AsNoTracking()
                    .ToListAsync();
                
                Console.WriteLine($"DataSeeder: Total de motos no banco: {todasMotos.Count}");
                
                if (todasMotos.Count > 0)
                {
                    var primeira = todasMotos.First();
                    Console.WriteLine($"DataSeeder: Primeira moto - Id: {primeira.Id}, Placa: {primeira.Placa}, Modelo: {(primeira.Modelo ?? "NULL")}, Ano: {primeira.Ano}");
                }
                
                // Atualiza Motos que não têm Modelo (propriedade obrigatória)
                // No Oracle, NULL pode ser tratado diferente, então vamos usar SQL direto
                var motosSemModelo = todasMotos
                    .Where(m => string.IsNullOrEmpty(m.Modelo))
                    .ToList();

                if (motosSemModelo.Any())
                {
                    Console.WriteLine($"DataSeeder: Encontradas {motosSemModelo.Count} motos sem Modelo. Atualizando...");
                    
                    // Busca novamente com tracking para poder atualizar
                    var motosParaAtualizar = await context.Motos
                        .Where(m => string.IsNullOrEmpty(m.Modelo))
                        .ToListAsync();
                    
                    foreach (var moto in motosParaAtualizar)
                    {
                        // Define um modelo padrão baseado na placa ou um genérico
                        moto.Modelo = $"Modelo {moto.Placa}";
                    }
                    
                    await context.SaveChangesAsync();
                    Console.WriteLine($"DataSeeder: {motosParaAtualizar.Count} motos atualizadas com sucesso.");
                }
                else
                {
                    Console.WriteLine("DataSeeder: Todas as motos já têm Modelo definido.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DataSeeder: Erro ao atualizar dados antigos: {ex.Message}");
                Console.WriteLine($"DataSeeder: Stack trace: {ex.StackTrace}");
                System.Diagnostics.Debug.WriteLine($"DataSeeder: Erro ao atualizar dados antigos: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"DataSeeder: Stack trace: {ex.StackTrace}");
            }
        }
    }
}

