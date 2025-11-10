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
    public class MotoServiceTests
    {
        private readonly Mock<IMotoRepository> _mockRepository;
        private readonly MotoService _service;
        private readonly IMapper _mapper;

        public MotoServiceTests()
        {
            _mockRepository = new Mock<IMotoRepository>();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            _mapper = mapperConfig.CreateMapper();
            _service = new MotoService(_mockRepository.Object, _mapper);
        }

        [Fact]
        public async Task CreateMotoAsync_ShouldReturnCreatedMotoDto()
        {
            // Arrange
            var inputDto = new MotoDto { Placa = "ABC1234", Modelo = "Titan 150", Ano = 2020 };
            var motoModel = _mapper.Map<Moto>(inputDto);
            motoModel.MotoId = 1;
            
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Moto>()))
                           .ReturnsAsync(motoModel);

            // Act
            var result = await _service.CreateMotoAsync(inputDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.MotoId);
            Assert.Equal("ABC1234", result.Placa);
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Moto>()), Times.Once);
        }
        
        [Fact]
        public async Task GetMotoByIdAsync_WhenMotoExists_ShouldReturnMotoDto()
        {
            // Arrange
            var moto = new Moto { MotoId = 5, Placa = "XYZ9876", Modelo = "Fazer 250", Ano = 2022 };
            _mockRepository.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(moto);

            // Act
            var result = await _service.GetMotoByIdAsync(5);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("XYZ9876", result.Placa);
        }
        
        [Fact]
        public async Task GetMotoByIdAsync_WhenMotoDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Moto)null!);

            // Act
            var result = await _service.GetMotoByIdAsync(99);

            // Assert
            Assert.Null(result);
        }
        
        [Fact]
        public async Task DeleteMotoAsync_ShouldCallDeleteAndReturnTrue()
        {
            // Arrange
            _mockRepository.Setup(r => r.DeleteAsync(2)).ReturnsAsync(true);
            
            // Act
            var result = await _service.DeleteMotoAsync(2);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(r => r.DeleteAsync(2), Times.Once);
        }
    }
}