using Castle.Core.Resource;
using Domain.Models;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace UnitTest.Domain.UnitTest
{
    public class BookingEntityTest
    {
        [Fact]
        public void BookingCreation_ShouldPass_WhenGivenCorrectInformation()
        {
            // Arrange
            int guestId = 1;
            int ressourceId = 1;
            DateOnly startDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2));
            decimal totalPrice = 5000;

            // Act
            Booking booking = new Booking(guestId, ressourceId, startDate, endDate, totalPrice);

            // Assert
            Assert.Equal(guestId, booking.GuestId);
            Assert.Equal(ressourceId, booking.ResourceId);
            Assert.Equal(startDate, booking.StartDate);
            Assert.Equal(endDate, booking.EndDate);
            Assert.Equal(totalPrice, booking.TotalPrice);
        }

        [Fact]
        public void BookingCreation_ShouldFail_WhenGivenStartDateTimeInThePast()
        {
            // Arrange
            int guestId = 1;
            int ressourceId = 1;
            DateOnly startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));
            DateOnly endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2));
            decimal totalPrice = 5000;

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => new Booking(guestId, ressourceId, startDate, endDate, totalPrice));
        }

        [Fact]
        public void BookingCreation_ShouldFail_WhenGivenStartDateIsEqualToEndDate()
        {
            // Arrange
            int guestId = 1;
            int ressourceId = 1;
            DateOnly startDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly endDate = DateOnly.FromDateTime(DateTime.Now);
            decimal totalPrice = 5000;

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => new Booking(guestId, ressourceId, startDate, endDate, totalPrice));
        }

        [Fact]
        public void BookingCreation_ShouldFail_WhenGivenStartDateIsBeforeEndDate()
        {
            // Arrange
            int guestId = 1;
            int ressourceId = 1;
            DateOnly startDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));
            decimal totalPrice = 5000;

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => new Booking(guestId, ressourceId, startDate, endDate, totalPrice));
        }

        [Fact]
        public void BookingCreation_ShouldFail_WhenGivenTotalPriceIsLessThanZero()
        {
            // Arrange
            int guestId = 1;
            int ressourceId = 1;
            DateOnly startDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2));
            decimal totalPrice = -1;

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => new Booking(guestId, ressourceId, startDate, endDate, totalPrice));
        }

        [Fact]
        public void Booking_ShouldFail_WhenStartDateIsGreatherThanEndDate()
        {
            // Arrange
            int guestId = 1;
            int resourceId = 1;
            DateOnly startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(3));
            DateOnly endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            decimal totalPrice = 1000;

            // Act
            Resource resource = new Resource(name: "Paradis", type: "Hytte", basePrice: 500, location: 5, description: "");

            // Assert
            Assert.Throws<ArgumentException>(() => new Booking(guestId, resourceId, startDate, endDate, totalPrice));
        }

        // Testing for totalPrice

        [Theory]
        [InlineData(1000, true)]
        [InlineData(1000.1, true)]
        [InlineData(1000.99, true)]
        [InlineData(1000.999, false)]
        [InlineData(1000.45678901234567890123456789, false)]
        public void Booking_ShouldValidateTotalPriceDecimalPlacesCorrectly(decimal totalPrice, bool pass)
        {
            // Arrange
            int guestId = 1;
            int resourceId = 1;
            DateOnly startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            DateOnly endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(7));

            // Act & Assert
            if (pass)
            {
                Booking booking = new Booking(guestId, resourceId, startDate, endDate, totalPrice);
                Assert.Equal(totalPrice, booking.TotalPrice);
            }
            else
            {
                Assert.Throws<ArgumentException>(() =>
                    new Booking(guestId, resourceId, startDate, endDate, totalPrice)
                );
            }
        }

        // Testing for method: GetNumberOfDecimals()

        [Theory]
        [InlineData (0, 100)]
        [InlineData (1, 100.1)]
        [InlineData (2, 100.11)]
        [InlineData (3, 100.111)]
        [InlineData (12, 123.456789012345)]
        public void GetNumberOfDecimals_ShouldReturnCorrectDecimalCount(int expected, decimal totalPrice)
        {
            // Arrange
            BookingEntityTestClass bookingEntityTestClass = new BookingEntityTestClass();

            // Act
            int result = bookingEntityTestClass.GetNumberOfDecimals(totalPrice);

            // Assert

            Assert.Equal(expected, result);
        }
    }
}



