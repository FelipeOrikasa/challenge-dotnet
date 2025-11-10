using Moq;
using Mottu.Api.Mappers;
using Mottu.Api.Models;
using Mottu.Api.Repositories.Interfaces;
using Mottu.Api.Services;
using Xunit;
using System.Threading.Tasks;
using AutoMapper;
using Mottu.Api.Data;
using Mottu.Api.DTOs.LocalizacaoDtos;
using System;
using System.Collections.Generic;

namespace Mottu.Api.Tests.Unit
{
    public class LocalizacaoServiceTests
    {
        private readonly Mock<ILocalizacaoRepository> _mockLocalizacaoRepository;
        private readonly Mock<IMotoRepository> _mockMotoRepository;
        private readonly Mock<ISensorRepository> _mockSensorRepository;
        private readonly Mock<AppDbContext> _mockContext;
        private readonly LocalizacaoService _service;
        private readonly IMapper _mapper;

        public LocalizacaoServiceTests()
        {
            _mockLocalizacaoRepository = new Mock<ILocalizacaoRepository>();
            _mockMotoRepository = new Mock<IMotoRepository>();
            _mockSensorRepository = new Mock<ISensorRepository>();
            _mockContext = new Mock<AppDbContext>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            _mapper = mapperConfig.CreateMapper();

            _service = new LocalizacaoService(
                _mockLocalizacaoRepository.Object,
                _mockMotoRepository.Object,
                _mockSensorRepository.Object,
                _mapper,
                _mockContext.Object);
        }

        [Fact]
        public async Task CreateAsync_WhenMotoAndSensorExist_ShouldCreateLocalizacao()
        {
            // Arrange
            var createDto = new CreateLocalizacaoDto { MotoId = 1, SensorId = 1, Latitude = 10.0m, Longitude = 20.0m };
            var moto = new Moto { MotoId = 1 };
            var sensor = new Sensor { SensorId = 1 };
            
            _mockMotoRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(moto);
            _mockSensorRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(sensor);
            
            _mockLocalizacaoRepository.Setup(r => r.AddAsync(It.IsAny<Localizacao>()))
                .Callback<Localizacao>(l => l.LocalizacaoId = 5)
                .Returns(Task.CompletedTask);
            _mockLocalizacaoRepository.Setup(r => r.GetByIdAsync(5))
                .ReturnsAsync(new Localizacao { LocalizacaoId = 5, MotoId = 1, SensorId = 1 });
            
            // Act
            var result = await _service.CreateAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.LocalizacaoId);
            _mockLocalizacaoRepository.Verify(r => r.AddAsync(It.IsAny<Localizacao>()), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenMotoDoesNotExist_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var createDto = new CreateLocalizacaoDto { MotoId = 99, SensorId = 1, Latitude = 10.0m, Longitude = 20.0m };
            var sensor = new Sensor { SensorId = 1 };

            _mockMotoRepository.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Moto)null!);
            _mockSensorRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(sensor);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.CreateAsync(createDto));
            
            _mockLocalizacaoRepository.Verify(r => r.AddAsync(It.IsAny<Localizacao>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_WhenSensorDoesNotExist_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var createDto = new CreateLocalizacaoDto { MotoId = 1, SensorId = 99, Latitude = 10.0m, Longitude = 20.0m };
            var moto = new Moto { MotoId = 1 };

            _mockMotoRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(moto);
            _mockSensorRepository.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Sensor)null!);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.CreateAsync(createDto));

            _mockLocalizacaoRepository.Verify(r => r.AddAsync(It.IsAny<Localizacao>()), Times.Never);
        }
    }
}