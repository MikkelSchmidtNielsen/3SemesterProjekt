using Application.Factories;
using Common.ResultInterfaces;
using Domain.Models;
using Domain.ModelsDto;

namespace UnitTest.Application.UnitTest.FactoryTest
{
    public class GuestFactoryTest
    {
        [Fact]
        public void FactoryCreation_ShouldReturnSuccess_WhenGivenCorrectGuest()
        {
            // Arrange
            GuestFactory factory = new GuestFactory();
            CreatedGuestDto dto = new CreatedGuestDto
            {
                FirstName = "Mikkel",
                LastName = "Schmidt",
                PhoneNumber = 12345678,
                Email = "test@test.com",
                Country = "Danmark",
                Language = "Dansk",
                Address = "Vejvej 12"
            };

            // Act
            IResult<Guest> result = factory.Create(dto);
            IResultSuccess<Guest> success = result.GetSuccess();

            // Assert
            Assert.True(result.IsSucces());
            Assert.Equal(dto.FirstName, success.OriginalType.FirstName);
            Assert.Equal(dto.LastName, success.OriginalType.LastName);
            Assert.Equal(dto.PhoneNumber, success.OriginalType.PhoneNumber);
            Assert.Equal(dto.Email, success.OriginalType.Email);
            Assert.Equal(dto.Country, success.OriginalType.Country);
            Assert.Equal(dto.Language, success.OriginalType.Language);
            Assert.Equal(dto.Address, success.OriginalType.Address);
        }
    }
}
