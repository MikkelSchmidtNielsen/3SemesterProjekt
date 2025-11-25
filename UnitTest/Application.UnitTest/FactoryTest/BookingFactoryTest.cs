using Application.Factories;
using Common.ResultInterfaces;
using Domain.Models;
using Domain.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Application.UnitTest.FactoryTest
{
    public class BookingFactoryTest
    {
		[Fact]
		public void FactoryCreation_ShouldReturnSuccess_WhenGivenCorrectBooking()
        public void BookingFactoryCreation_ShouldReturnSuccess_WhenGivenCorrectInfo()
		{
			// Arrange
			BookingFactory factory = new BookingFactory();
			BookingCreateFactoryDto dto = new BookingCreateFactoryDto
            GuestInputDomainDto dto = new GuestInputDomainDto
            {
				Id = 1,
				ResourceId = 1,
                Guest = new Guest("Allan", "Allansen", 12345678, "test@test.dk", "Denmark", "Danish", "Skovgade 12"),
                Resource = new Resource(1, "Gaia", "Telt", 100),
				StartDate = DateOnly.FromDateTime(DateTime.Now),
				EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
				TotalPrice = 100
            };

			// Act
			IResult<Booking> result = factory.Create(dto);
			IResultSuccess<Booking> succes = result.GetSuccess();
            IResultSuccess<Booking> success = result.GetSuccess();

			// Assert
			Assert.True(result.IsSucces());
			Assert.Equal(dto.GuestId, succes.OriginalType.GuestId);
			Assert.Equal(dto.ResourceId, succes.OriginalType.ResourceId);
			Assert.Equal(dto.StartDate, succes.OriginalType.StartDate);
			Assert.Equal(dto.EndDate, succes.OriginalType.EndDate);
			Assert.Equal(dto.TotalPrice, succes.OriginalType.TotalPrice);

            Assert.Equal(dto.Resource.Id, success.OriginalType.ResourceId);
            Assert.Equal(dto.Guest.FirstName, success.OriginalType.GuestName);
            Assert.Equal(dto.StartDate, success.OriginalType.StartDate);
            Assert.Equal(dto.EndDate, success.OriginalType.EndDate);
            Assert.Equal(dto.TotalPrice, success.OriginalType.TotalPrice);
		}
	}
}
