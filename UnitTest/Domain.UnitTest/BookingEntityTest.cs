using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Domain.UnitTest
{
    public class BookingEntityTest
    {
        [Fact]
        public void BookingCreation_ShouldPass_WhenGivenCorrectInformation()
        {
            // Arrange
            string guestName = "John";
            DateOnly startDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2));
            decimal totalPrice = 5000;

            // Act
            Booking booking = new Booking(guestName, startDate, endDate, totalPrice);

            // Assert
            Assert.Equal(startDate, booking.StartDate);
            Assert.Equal(endDate, booking.EndDate);
            Assert.Equal(totalPrice, booking.TotalPrice);
        }

        [InlineData("")]
        [InlineData(null)]
        public void BookingCreation_ShouldFail_WhenGivenGuestNameIsNullOrEmpty(string guestName)
        {
            // Arrange
            DateOnly startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));
            DateOnly endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2));
            decimal totalPrice = 5000;

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => new Booking(guestName, startDate, endDate, totalPrice));
        }

        [Fact]
        public void BookingCreation_ShouldFail_WhenGivenStartDateTimeInThePast()
        {
            // Arrange
            string guestName = "John";
            DateOnly startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));
            DateOnly endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2));
            decimal totalPrice = 5000;

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => new Booking(guestName, startDate, endDate, totalPrice));
        }

        [Fact]
        public void BookingCreation_ShouldFail_WhenGivenStartDateIsEqualToEndDate()
        {
            // Arrange
            string guestName = "John";
            DateOnly startDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly endDate = DateOnly.FromDateTime(DateTime.Now);
            decimal totalPrice = 5000;

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => new Booking(guestName, startDate, endDate, totalPrice));
        }

        [Fact]
        public void BookingCreation_ShouldFail_WhenGivenStartDateIsBeforeEndDate()
        {
            // Arrange
            string guestName = "John";
            DateOnly startDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));
            decimal totalPrice = 5000;

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => new Booking(guestName, startDate, endDate, totalPrice));
        }

        [Fact]
        public void BookingCreation_ShouldFail_WhenGivenTotalPriceIsLessThanZero()
        {
            // Arrange
            string guestName = "John";
            DateOnly startDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2));
            decimal totalPrice = -1;

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => new Booking(guestName, startDate, endDate, totalPrice));
        }
    }
}
