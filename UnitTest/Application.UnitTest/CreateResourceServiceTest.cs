using Application.RepositoryInterfaces;
using Application.Services.Command;
using Common;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.ModelsDto;

namespace UnitTest.Application.UnitTest
{
    public class CreateResourceServiceTest
    {
        [Fact]
        public static void ResourceCreation_ShouldPass_WhenGivenCorrectInfo()
        {
            // Arrange
            var dto = new CreateResourceDto
            {
                Name = "Strandhytten",
                Type = "Hytte",
                BasePrice = 599.95M,
                Description = "Seniorvenlig hytte"
            };
            

        }
    }
}
