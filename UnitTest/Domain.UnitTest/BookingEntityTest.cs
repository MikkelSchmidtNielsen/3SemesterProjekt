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
        public void GuestBooking_ShouldCreate_WhenGivenCorrectInformation()
        {
            // Arrange
            int guestId = 1;
            int resourceId = 1;

            DateOnly startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            DateOnly endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(7));
            decimal totalPrice = 1000;

            // Act
            Guest guest = new Guest("Allan", "Allansen", 12345678, "aa@aa.dk", "Danmark", "Dansk", "Allanvej 11");
            Resource resource = new Resource(resourceId, "Paradis", "Hytte", 500);
            Booking booking = new Booking(guestId, resource.Id, guest.FirstName + guest.LastName, startDate, endDate, totalPrice);

            // Assert
            Assert.Equal(resourceId, booking.ResourceId);
            Assert.Equal(guest.FirstName + guest.LastName, booking.GuestName);
            Assert.Equal(startDate, booking.StartDate);
            Assert.Equal(endDate, booking.EndDate);
            Assert.Equal(totalPrice, booking.TotalPrice);
        }
        [Fact]
        public void GuestBooking_ShouldFail_WhenStartDateIsInThePast()
        {
            // Arrange
            int guestId = 1;
            int resourceId = 1;

            DateOnly startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));
            DateOnly endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(7));
            decimal totalPrice = 1000;

            // Act
            Guest guest = new Guest("Allan", "Allansen", 12345678, "aa@aa.dk", "Danmark", "Dansk", "Allanvej 11");
            Resource resource = new Resource(resourceId, "Paradis", "Hytte", 500);

            // Assert
            Assert.Throws<Exception>(() => new Booking(guest.Id, resource.Id, guest.FirstName + guest.LastName,
                startDate, endDate, totalPrice));
        }
        [Fact]
        public void GuestBooking_ShouldFail_WhenStartDateIsGreatherThanEndDate()
        {
            // Arrange
            int guestId = 1;
            int resourceId = 1;

            DateOnly startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(3));
            DateOnly endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            decimal totalPrice = 1000;

            // Act
            Guest guest = new Guest("Allan", "Allansen", 12345678, "aa@aa.dk", "Danmark", "Dansk", "Allanvej 11");
            Resource resource = new Resource(resourceId, "Paradis", "Hytte", 500);

            // Assert
            Assert.Throws<Exception>(() => new Booking(guest.Id, resource.Id, guest.FirstName + guest.LastName,
                startDate, endDate, totalPrice));
        }
        [Fact]
        public void GuestBooking_ShouldFail_WhenStartDateAndEndDateAreTheSame()
        {
            // Arrange
            int guestId = 1;
            int resourceId = 1;

            DateOnly startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            DateOnly endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            decimal totalPrice = 1000;

            // Act
            Guest guest = new Guest("Allan", "Allansen", 12345678, "aa@aa.dk", "Danmark", "Dansk", "Allanvej 11");
            Resource resource = new Resource(resourceId, "Paradis", "Hytte", 500);

            // Assert
            Assert.Throws<Exception>(() => new Booking(guest.Id, resource.Id, guest.FirstName + guest.LastName,
                startDate, endDate, totalPrice));
        }
        [Fact]
        public void GuestBooking_ShouldFail_WhenTotalPriceIsNegative()
        {
            // Arrange
            int guestId = 1;
            int resourceId = 1;

            DateOnly startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            DateOnly endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(7));
            decimal totalPrice = -100;

            // Act
            Guest guest = new Guest("Allan", "Allansen", 12345678, "aa@aa.dk", "Danmark", "Dansk", "Allanvej 11");
            Resource resource = new Resource(resourceId, "Paradis", "Hytte", 500);

            // Assert
            Assert.Throws<Exception>(() => new Booking(guest.Id, resource.Id, guest.FirstName + guest.LastName,
                startDate, endDate, totalPrice));
        }
        [Fact]
        public void GuestBooking_ShouldFail_WhenTotalPriceHasMoreThanTwoDecimals()
        {
            // Arrange
            int guestId = 1;
            int resourceId = 1;

            DateOnly startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            DateOnly endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(7));
            decimal totalPrice = 1000.999m;

            // Act
            Guest guest = new Guest("Allan", "Allansen", 12345678, "aa@aa.dk", "Danmark", "Dansk", "Allanvej 11");
            Resource resource = new Resource(resourceId, "Paradis", "Hytte", 500);

            // Assert
            Assert.Throws<Exception>(() => new Booking(guest.Id, resource.Id, guest.FirstName + guest.LastName,
                startDate, endDate, totalPrice));
        }
        [Fact]
        public void GuestBooking_ShouldPass_WhenTotalPriceHasTwoDecimals()
        {
            // Arrange
            int guestId = 1;
            int resourceId = 1;

            DateOnly startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            DateOnly endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(7));
            decimal totalPrice = 1000.99m;

            // Act
            Guest guest = new Guest("Allan", "Allansen", 12345678, "aa@aa.dk", "Danmark", "Dansk", "Allanvej 11");
            Resource resource = new Resource(resourceId, "Paradis", "Hytte", 500);
            Booking booking = new Booking(guest.Id, resource.Id, guest.FirstName + guest.LastName, startDate, endDate, totalPrice);

            // Assert
            Assert.Equal(totalPrice, booking.TotalPrice);
        }
        [Fact]
        public void GuestBooking_ShouldPass_WhenTotalPriceHasOneDecimal()
        {
            // Arrange
            int guestId = 1;
            int resourceId = 1;

            DateOnly startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            DateOnly endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(7));
            decimal totalPrice = 1000.9m;

            // Act
            Guest guest = new Guest("Allan", "Allansen", 12345678, "aa@aa.dk", "Danmark", "Dansk", "Allanvej 11");
            Resource resource = new Resource(resourceId, "Paradis", "Hytte", 500);
            Booking booking = new Booking(guest.Id, resource.Id, guest.FirstName + guest.LastName, startDate, endDate, totalPrice);

            // Assert
            Assert.Equal(totalPrice, booking.TotalPrice);
        }
        [Fact]
        public void GuestBooking_ShouldPass_WhenTotalPriceHasZeroDecimals()
        {
            // Arrange
            int guestId = 1;
            int resourceId = 1;

            DateOnly startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            DateOnly endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(7));
            decimal totalPrice = 1000;

            // Act
            Guest guest = new Guest("Allan", "Allansen", 12345678, "aa@aa.dk", "Danmark", "Dansk", "Allanvej 11");
            Resource resource = new Resource(resourceId, "Paradis", "Hytte", 500);
            Booking booking = new Booking(guest.Id, resource.Id, guest.FirstName + guest.LastName, startDate, endDate, totalPrice);

            // Assert
            Assert.Equal(totalPrice, booking.TotalPrice);
        }
    }
}



