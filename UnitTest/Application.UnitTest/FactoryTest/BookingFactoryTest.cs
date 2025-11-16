using Application.Factories;
using Common.ResultInterfaces;
using Domain.Models;

namespace UnitTest.Application.UnitTest.FactoryTest
{
    public class BookingFactoryTest
    {
		[Fact]
		public void FactoryCreation_ShouldReturnSuccess_WhenGivenCorrectBooking()
		{
			// Arrange
			BookingFactory factory = new BookingFactory();
			int guestId = 1;
			int resourceId = 1;
			var startDate = DateOnly.FromDateTime(DateTime.Now);
			var endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
			decimal totalPrice = 100;

			// Act
			IResult<Booking> result = factory.Create(guestId, resourceId, startDate, endDate, totalPrice);
			IResultSuccess<Booking> succes = result.GetSuccess();

			// Assert
			Assert.True(result.IsSucces());
			Assert.Equal(guestId, succes.OriginalType.GuestId);
			Assert.Equal(resourceId, succes.OriginalType.ResourceId);
			Assert.Equal(startDate, succes.OriginalType.StartDate);
			Assert.Equal(endDate, succes.OriginalType.EndDate);
			Assert.Equal(totalPrice, succes.OriginalType.TotalPrice);
		}
	}
}
