using Moq;
using Mottu.Api.Mappers;
using Mottu.Api.Models;
using Mottu.Api.Repositories.Interfaces;
using Mottu.Api.Services;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Mottu.Api.Data;
using System;

namespace Mottu.Api.Tests.Unit
{
    public class FilialServiceTests
    {
        private readonly Mock<IFilialRepository> _mockFilialRepository;
        private readonly Mock<IPatioRepository> _mockPatioRepository;
        private readonly Mock<AppDbContext> _mockContext;
        private readonly FilialService _service;
        private readonly IMapper _mapper;

        public FilialServiceTests()
        {
            _mockFilialRepository = new Mock<IFilialRepository>();
            _mockPatioRepository = new Mock<IPatioRepository>();
            _mockContext = new Mock<AppDbContext>(); 
            
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            _mapper = mapperConfig.CreateMapper();

            _service = new FilialService(
                _mockFilialRepository.Object, 
                _mockPatioRepository.Object, 
                _mapper, 
                _mockContext.Object);
        }

        [Fact]
        public async Task DeleteAsync_WhenNoPatiosExist_ShouldSucceed()
        {
            // Arrange
            var filialId = 1;
            var filial = new Filial { FilialId = filialId, Nome = "Filial Limpa" };

            _mockFilialRepository.Setup(r => r.GetByIdAsync(filialId)).ReturnsAsync(filial);
            _mockPatioRepository.Setup(r => r.GetCountByFilialAsync(filialId)).ReturnsAsync(0);

            // Act
            await _service.DeleteAsync(filialId);

            // Assert
            _mockFilialRepository.Verify(r => r.Delete(filial), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenPatiosExist_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var filialId = 2;
            var filial = new Filial { FilialId = filialId, Nome = "Filial Com Pátios" };

            _mockFilialRepository.Setup(r => r.GetByIdAsync(filialId)).ReturnsAsync(filial);
            _mockPatioRepository.Setup(r => r.GetCountByFilialAsync(filialId)).ReturnsAsync(3); 

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.DeleteAsync(filialId));
            
            // Assert que a exclusão não foi chamada
            _mockFilialRepository.Verify(r => r.Delete(It.IsAny<Filial>()), Times.Never);
        }
        
        [Fact]
        public async Task DeleteAsync_WhenFilialDoesNotExist_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var filialId = 99;
            _mockFilialRepository.Setup(r => r.GetByIdAsync(filialId)).ReturnsAsync((Filial)null!);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(filialId));
        }
    }
}