using Application.Factories;
using Application.RepositoryInterfaces;
using Castle.Core.Resource;
using Common;
using Common.ResultInterfaces;
using Domain.Models;
using Domain.ModelsDto;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Application.UnitTest
{
    public class ResourceFactoryTest
    {
        [Fact]
        public async Task ResourceCreation_ShouldPass_WhenGivenCorrectInfo()
        {
            // Arrange

            CreateResourceDto dto1 = new CreateResourceDto
            {
                Name = "Luksushytte nr. 5",
                Type = "Hytte",
                BasePrice = 899.95M,
                Description = null
            };
            Mock<IResourceRepository> repository = new Mock<IResourceRepository>();
            repository.Setup(x => x.GetResourceByResourceNameAsync(dto1.Name)).ReturnsAsync(Result<Resource>.Error(null, new Exception("En ressource med dette navn eksisterer ikke.")));
            repository.Setup(y => y.GetResourceByLocationAsync(dto1.Location)).ReturnsAsync(Result<Resource>.Error(null, new Exception("Der kunne ikke findes en ressource med det valgte pladsnr.")));
            ResourceFactory resourceFactory = new ResourceFactory(repository.Object);

            // Act
            
            var result = await resourceFactory.CreateResourceAsync(dto1);

            // Assert

            Assert.True(result.IsSucces());

        }
        [Fact]
        public async Task ResourceCreation_ShouldFail_IfResourceWithSameNameAlreadyExists()
        {
            // Arrange
            Resource existingResource = new Resource("Luksushytte nr. 5", "Hytte", 899.95M, 4, null);
            CreateResourceDto newResourceDto = new CreateResourceDto
            {
                Name = "Luksushytte nr. 5",
                Type = "Teltplads",
                BasePrice = 999.95M,
                Location = 7,
                Description = null
            };

            Mock<IResourceRepository> repository = new Mock<IResourceRepository>();
            ResourceFactory resourceFactory = new ResourceFactory(repository.Object);
            repository.Setup(x => x.GetResourceByResourceNameAsync(newResourceDto.Name)).ReturnsAsync(Result<Resource>.Success(existingResource));


            // Act
            var result2 = await resourceFactory.CreateResourceAsync(newResourceDto);

            // Assert
            Assert.False(result2.IsSucces());
        }
        [Fact]
        public async Task ResourceCreation_ShouldFail_IfThereIsAnExistingResourceOnGivenLocation()
        {
            // Arrange
            Resource existingResource = new Resource("Luksushytte nr. 5", "Hytte", 899.95M, 4, null);
            CreateResourceDto newResourceDto = new CreateResourceDto
            {
                Name = "Luksushytte nr. 5",
                Type = "Teltplads",
                BasePrice = 999.95M,
                Location = 4,
                Description = null
            };

            Mock<IResourceRepository> repository = new Mock<IResourceRepository>();
            ResourceFactory resourceFactory = new ResourceFactory(repository.Object);
            repository.Setup(x => x.GetResourceByResourceNameAsync(newResourceDto.Name)).ReturnsAsync(Result<Resource>.Success(existingResource));
            repository.Setup(x => x.GetResourceByLocationAsync(newResourceDto.Location)).ReturnsAsync(Result<Resource>.Success(existingResource));


            // Act
            var result2 = await resourceFactory.CreateResourceAsync(newResourceDto);

            // Assert
            Assert.False(result2.IsSucces());
        }
    }
}
