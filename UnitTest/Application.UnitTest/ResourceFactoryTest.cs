using Application.Factories;
using Application.RepositoryInterfaces;
using Castle.Core.Resource;
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
            repository.Setup(x => x.GetResourceByResourceNameAsync(dto1.Name));
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
            CreateResourceDto dto1 = new CreateResourceDto
            {
                Name = "Luksushytte nr. 5",
                Type = "Hytte",
                BasePrice = 899.95M,
                Description = null
            };
            Resource resource = new Resource(dto1);
            CreateResourceDto dto2 = new CreateResourceDto
            {
                Name = "Luksushytte nr. 5",
                Type = "Teltplads",
                BasePrice = 999.95M,
                Description = null
            };

            Mock<IResourceRepository> repository = new Mock<IResourceRepository>();
            ResourceFactory resourceFactory = new ResourceFactory(repository.Object);
            repository.Setup(x => x.GetResourceByResourceNameAsync(dto2.Name)).ReturnsAsync(resource);
            

            // Act
            var result2 = await resourceFactory.CreateResourceAsync(dto2);

            // Assert
            Assert.False(result2.IsSucces());
        }
    }
}
