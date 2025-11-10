using Moq;
using Mottu.Api.Mappers;
using Mottu.Api.Models;
using Mottu.Api.Repositories.Interfaces;
using Mottu.Api.Services;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using System;

namespace Mottu.Api.Tests.Unit
{
    public class SensorServiceTests
    {
        private readonly Mock<ISensorRepository> _mockRepository;
        private readonly SensorService _service;
        private readonly IMapper _mapper;

        public SensorServiceTests()
        {
            _mockRepository = new Mock<ISensorRepository>();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            _mapper = mapperConfig.CreateMapper();
            _service = new SensorService(_mockRepository.Object, _mapper);
        }

        [Fact]
        public async Task CreateSensorAsync_ShouldReturnCreatedSensorDto()
        {
            // Arrange
            var inputDto = new SensorDto { Descricao = "Sensor GPS Teste", MotoId = 1 };
            var sensorModel = _mapper.Map<Sensor>(inputDto);
            sensorModel.SensorId = 1;
            
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Sensor>()))
                           .ReturnsAsync(sensorModel);

            // Act
            var result = await _service.CreateSensorAsync(inputDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.SensorId);
            Assert.Equal("Sensor GPS Teste", result.Descricao);
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Sensor>()), Times.Once);
        }
        
        [Fact]
        public async Task GetAllSensorsAsync_ShouldReturnListOfSensorDtos()
        {
            // Arrange
            var sensors = new List<Sensor>
            {
                new Sensor { SensorId = 1, Descricao = "A", MotoId = 1 },
                new Sensor { SensorId = 2, Descricao = "B", MotoId = 2 }
            };

            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(sensors);

            // Act
            var result = await _service.GetAllSensorsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }
        
        [Fact]
        public async Task UpdateSensorAsync_WhenSensorExists_ShouldReturnUpdatedDto()
        {
            // Arrange
            var existingSensor = new Sensor { SensorId = 3, Descricao = "Velho", MotoId = 1 };
            var updatedDto = new SensorDto { SensorId = 3, Descricao = "Novo", MotoId = 1 };
            var updatedModel = _mapper.Map<Sensor>(updatedDto);

            _mockRepository.Setup(r => r.GetByIdAsync(3)).ReturnsAsync(existingSensor);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Sensor>())).ReturnsAsync(updatedModel);

            // Act
            var result = await _service.UpdateSensorAsync(updatedDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Novo", result.Descricao);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Sensor>()), Times.Once);
        }
        
        [Fact]
        public async Task UpdateSensorAsync_WhenSensorDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var nonExistentDto = new SensorDto { SensorId = 99, Descricao = "Não Existe", MotoId = 1 };
            _mockRepository.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Sensor)null!);
            
            // Act
            var result = await _service.UpdateSensorAsync(nonExistentDto);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Sensor>()), Times.Never);
        }
    }
}