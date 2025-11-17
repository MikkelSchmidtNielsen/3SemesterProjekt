using Application.Factories;
using Common.ResultInterfaces;
using Domain.Models;
using Domain.ModelsDto;

namespace UnitTest.Application.UnitTest.FactoryTest
{
    public class BookingFactoryTest
    {
		[Fact]
		public void FactoryCreation_ShouldReturnSuccess_WhenGivenCorrectBooking()
		{
			// Arrange
			BookingFactory factory = new BookingFactory();
			CreatedBookingDto dto = new CreatedBookingDto
			{
				Id = 1,
				ResourceId = 1,
				StartDate = DateOnly.FromDateTime(DateTime.Now),
				EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
				TotalPrice = 100
            };

			// Act
			IResult<Booking> result = factory.Create(dto);
			IResultSuccess<Booking> succes = result.GetSuccess();

			// Assert
			Assert.True(result.IsSucces());
			Assert.Equal(dto.GuestId, succes.OriginalType.GuestId);
			Assert.Equal(dto.ResourceId, succes.OriginalType.ResourceId);
			Assert.Equal(dto.StartDate, succes.OriginalType.StartDate);
			Assert.Equal(dto.EndDate, succes.OriginalType.EndDate);
			Assert.Equal(dto.TotalPrice, succes.OriginalType.TotalPrice);
		}
	}
}
