using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Domain.UnitTest
{
	public class GuestEntityTest
	{
		[Fact]
		public void GuestCreation_ShouldCreateGuest_WhenDataIsValid()
		{
			// Arrange
			string firstName = "Dansker";
			string? lastName = "Et";
			int phoneNumber = 12345678;
			string? email = "test@test.com";
			string? country = "Denmark";
			string? language = "Dansk";
			string? address = "Testvej 1";

			// Act
			Guest guest = new Guest(firstName, lastName, phoneNumber, email, country, language, address);

			// Assert
			Assert.Equal(firstName, guest.FirstName);
			Assert.Equal(lastName, guest.LastName);
			Assert.Equal(phoneNumber, guest.PhoneNumber);
			Assert.Equal(email, guest.Email);
			Assert.Equal(country, guest.Country);
			Assert.Equal(language, guest.Language);
			Assert.Equal(address, guest.Address);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("   ")]
		public void GuestCreation_ShouldFail_WhenFirstNameIsNullOrWhitespace(string? firstName)
		{
			// Arrange
			string? lastName = "Et";
			int phoneNumber = 12345678;
			string? email = "test@test.com";
			string? country = "Denmark";
			string? language = "Dansk";
			string? address = "Testvej 1";

			// Act & Assert
			Assert.Throws<ArgumentException>(() =>
				new Guest(firstName!, lastName, phoneNumber, email, country, language, address));
		}

		[Fact]
		public void GuestCreation_ShouldFail_WhenPhoneNumberIsNegative()
		{
			// Arrange
			string firstName = "Dansker";
			string? lastName = "Et";
			int phoneNumber = -1;
			string? email = "test@test.com";
			string? country = "Denmark";
			string? language = "Dansk";
			string? address = "Testvej 1";

			// Act & Assert
			Assert.Throws<ArgumentException>(() =>
				new Guest(firstName, lastName, phoneNumber, email, country, language, address));
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("   ")]
		public void GuestCreation_ShouldNotFail_WhenEmailIsNullOrWhiteSpace(string email)
		{
			// Arrange
			string firstName = "Dansker";
			string? lastName = "Et";
			int phoneNumber = 12345678;
			string? country = "Denmark";
			string? language = "da";
			string? address = "Testvej 1";

			// Act
			Guest guest = new Guest(firstName, lastName, phoneNumber, email, country, language, address);

			// Assert
			Assert.Equal(email, guest.Email);
		}

		[Fact]
		public void GuestCreation_ShouldFail_WhenEmailIsInvalid()
		{
			// Arrange
			string firstName = "Dansker";
			string? lastName = "Et";
			int phoneNumber = 12345678;
			string? email = "invalid-email-without-at";
			string? country = "Denmark";
			string? language = "da";
			string? address = "Testvej 1";

			// Act + Assert
			Assert.Throws<ArgumentException>(() =>
				new Guest(firstName, lastName, phoneNumber, email, country, language, address));
		}
	}
}
