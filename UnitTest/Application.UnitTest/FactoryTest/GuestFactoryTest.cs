using Application.Factories;
using Common.ResultInterfaces;
using Domain.Models;

namespace UnitTest.Application.UnitTest.FactoryTest
{
    public class GuestFactoryTest
    {
		[Fact]
		public void FactoryCreation_ShouldReturnSuccess_WhenGivenCorrectGuest()
		{
			// Arrange
			GuestFactory factory = new GuestFactory();
			string firstName = "Mikkel";
			string lastName = "Schmidt";
			int phoneNumber = 12345678;
			string email = "test@test.com";
			string country = "Danmark";
			string language = "Dansk";
			string address = "Vejvej 12";

			// Act
			IResult<Guest> result = factory.Create(firstName, lastName, phoneNumber, email, country, language, address);
			IResultSuccess<Guest> success = result.GetSuccess();

			// Assert
			Assert.True(result.IsSucces());
			Assert.Equal(firstName, success.OriginalType.FirstName);
			Assert.Equal(lastName, success.OriginalType.LastName);
			Assert.Equal(phoneNumber, success.OriginalType.PhoneNumber);
			Assert.Equal(email, success.OriginalType.Email);
			Assert.Equal(country, success.OriginalType.Country);
			Assert.Equal(language, success.OriginalType.Language);
			Assert.Equal(address, success.OriginalType.Address);
		}
	}
}
