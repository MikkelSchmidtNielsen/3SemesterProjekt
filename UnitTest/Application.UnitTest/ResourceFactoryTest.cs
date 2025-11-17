using Application.Factories;
using Application.RepositoryInterfaces;
using Castle.Core.Resource;
using Common.ResultInterfaces;
using Domain.Models;
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
            
            int resourceId = 1;
            string resourceName = "Luksushytte nr. 5";
            string resourceType = "Hytte";
            double resourceBasePrice = 899.95;
            Resource resource = new Resource(resourceId, resourceName, resourceType, resourceBasePrice);
            Mock<IResourceRepository> repository = new Mock<IResourceRepository>();
            repository.Setup(x => x.GetResourceByResourceNameAsync(resource.Name));
            ResourceFactory resourceFactory = new ResourceFactory(repository.Object);

            // Act
            
            var result = await resourceFactory.CreateResourceAsync(resource);

            // Assert

            Assert.True(result.IsSucces());

        }
        [Fact]
        public async Task ResourceCreation_ShouldFail_IfResourceWithSameNameAlreadyExists()
        {
            // Arrange
            Resource resource1 = new Resource(1, "Luksushytte nr. 5", "Hytte", 899.95);
            Resource resource2 = new Resource(2, "Luksushytte nr. 5", "Hytte", 999.95);

            Mock<IResourceRepository> repository = new Mock<IResourceRepository>();
            repository.Setup(x => x.GetResourceByResourceNameAsync(resource2.Name)).ReturnsAsync(resource1);
            ResourceFactory resourceFactory = new ResourceFactory(repository.Object);

            // Act
            var result1 = await resourceFactory.CreateResourceAsync(resource1);
            var result2 = await resourceFactory.CreateResourceAsync(resource2);

            // Assert
            Assert.False(result2.IsSucces());
        }
    }
}
