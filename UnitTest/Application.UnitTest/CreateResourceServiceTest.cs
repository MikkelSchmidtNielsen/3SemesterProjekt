using Application.Factories;
using Application.RepositoryInterfaces;
using Application.Services.Command;
using Common;
using Domain.Models;
using Domain.ModelsDto;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Application.UnitTest
{
    public class CreateResourceServiceTest
    {
        [Fact]
        public async Task ResourceCreation_ShouldPass_WhenGivenCorrectInfo()
        {
            // Arrange
            var dto = new CreateResourceDto
            {
                Name = "Strandhytten",
                Type = "Hytte",
                BasePrice = 599.95M,
                Description = "Seniorvenlig hytte"
            };

            Mock<IResourceRepository> repository = new Mock<IResourceRepository>();
            repository.Setup(x => x.GetResourceByResourceNameAsync(dto.Name)).ReturnsAsync(Result<Resource>.Error(null, new Exception("En ressource med dette navn eksisterer ikke.")));
            repository.Setup(x => x.GetResourceByLocationAsync(dto.Location)).ReturnsAsync(Result<Resource>.Error(null, new Exception("Der findes allerede en ressource på denne placering.")));

            ResourceFactory resourceFactory = new ResourceFactory(repository.Object);

            // Act
            var result = await resourceFactory.CreateResourceAsync(dto);

            // Assert
            Assert.True(result.IsSucces());

        }
    }
}
