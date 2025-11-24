using Application.ApplicationDto.Command;
using Application.RepositoryInterfaces;
using Domain.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Application.UnitTest.ServiceTest
{
    
    public class GuestCreateBookingServiceTest
    {
        [Fact]
        public async Task GuestBookingCreation_ShouldPass_WhenGivenCorrectInformation()
        {
            // Arrange
            GuestInputDto dto = new GuestInputDto
            {
                Email = "test@test.dk",
                ResourceId = 1,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                TotalPrice = 100,
                Guest = new Guest("Allan", "Allansen", 12345678, "test@test.dk", "Denmark", "Danish", "Skovgade 12"),
                Resource = new Resource(1, "Gaia", "Telt", 100),
            };

            // Act
            Mock<IBookingRepository> repo = new Mock<IBookingRepository>();


            // Assert
        }

    }
}
