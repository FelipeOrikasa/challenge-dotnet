using Moq;
using Mottu.Api.Mappers;
using Mottu.Api.Models;
using Mottu.Api.Repositories.Interfaces;
using Mottu.Api.Services;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;

namespace Mottu.Api.Tests.Unit
{
    public class PatioServiceTests
    {
        private readonly Mock<IPatioRepository> _mockRepository;
        private readonly PatioService _service;
        private readonly IMapper _mapper;

        public PatioServiceTests()
        {
            _mockRepository = new Mock<IPatioRepository>();
            // Configuração do AutoMapper para simular a injeção
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            _mapper = mapperConfig.CreateMapper();

            // Instancia o serviço com as dependências mockadas e o mapper real
            _service = new PatioService(_mockRepository.Object, _mapper);
        }

        [Fact]
        public async Task CreatePatioAsync_ShouldReturnCreatedPatioDto()
        {
            // Arrange
            var inputDto = new PatioDto { Descricao = "Patio Teste", FilialId = 1 };
            var patioModel = _mapper.Map<Patio>(inputDto);
            patioModel.PatioId = 1; // Simula a ID após a criação

            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Patio>()))
                           .ReturnsAsync(patioModel);

            // Act
            var result = await _service.CreatePatioAsync(inputDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.PatioId);
            Assert.Equal("Patio Teste", result.Descricao);
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Patio>()), Times.Once);
        }

        [Fact]
        public async Task GetAllPatiosAsync_ShouldReturnListOfPatioDtos()
        {
            // Arrange
            var patios = new List<Patio>
            {
                new Patio { PatioId = 1, Descricao = "Patio A", FilialId = 1 },
                new Patio { PatioId = 2, Descricao = "Patio B", FilialId = 1 }
            };

            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(patios);

            // Act
            var result = await _service.GetAllPatiosAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, p => p.Descricao == "Patio A");
            _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task DeletePatioAsync_WhenPatioExists_ShouldCallDeleteAndReturnTrue()
        {
            // Arrange
            _mockRepository.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _service.DeletePatioAsync(1);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeletePatioAsync_WhenPatioDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            _mockRepository.Setup(r => r.DeleteAsync(99)).ReturnsAsync(false);

            // Act
            var result = await _service.DeletePatioAsync(99);

            // Assert
            Assert.False(result);
            _mockRepository.Verify(r => r.DeleteAsync(99), Times.Once);
        }
    }
}