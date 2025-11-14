using Domain.Models;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
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
        public void GuestBookingShouldCreateWhenGivenCorrectInformation()
        {
            // Arrange
            int guestId = 1;
            int resourceId = 1;

            DateOnly startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            DateOnly endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(7));
            decimal totalPrice = 100;

            // Act
            Guest guest = new Guest(guestId, "Allan", "Allansen", 12345678, "aa@aa.dk", "Danmark", "Dansk", "Allanvej 11");
            Resource resource = new Resource(resourceId, "Paradis", "Hytte", 500);
            Booking booking = new Booking(guest.Id, resource.Id, guest.FirstName + guest.LastName, startDate, endDate, totalPrice);

            // Assert
            Assert.Equal(guest.Id, booking.GuestId);
            Assert.Equal(resourceId, booking.ResourceId);
            Assert.Equal(guest.FirstName + guest.LastName, booking.GuestName);
            Assert.Equal(startDate, booking.StartDate);
            Assert.Equal(endDate, booking.EndDate);
            Assert.Equal(totalPrice, booking.TotalPrice);
        }
        [Fact]
        public void GuestBookingStartDateIsInThePast()
        {
            // Arrange
            int guestId = 1;
            int resourceId = 1;

            DateOnly startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));
            DateOnly endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(7));
            decimal totalPrice = 100;

            // Act
            Guest guest = new Guest(guestId, "Allan", "Allansen", 12345678, "aa@aa.dk", "Danmark", "Dansk", "Allanvej 11");
            Resource resource = new Resource(resourceId, "Paradis", "Hytte", 500);

            // Assert
            Assert.Throws<Exception>(() => new Booking(guest.Id, resource.Id, guest.FirstName + guest.LastName, 
                startDate, endDate, totalPrice));
        }
        [Fact]
        public void GuestBookingStartDateIsGreatherThanEndDate()
        {
            // Arrange
            int guestId = 1;
            int resourceId = 1;

            DateOnly startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(3));
            DateOnly endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            decimal totalPrice = 100;

            // Act
            Guest guest = new Guest(guestId, "Allan", "Allansen", 12345678, "aa@aa.dk", "Danmark", "Dansk", "Allanvej 11");
            Resource resource = new Resource(resourceId, "Paradis", "Hytte", 500);

            // Assert
            Assert.Throws<Exception>(() => new Booking(guest.Id, resource.Id, guest.FirstName + guest.LastName,
                startDate, endDate, totalPrice));
        }
        [Fact]
        public void GuestBookingStartDateIsSameDayAsEnddate()
        {
            // Arrange
            int guestId = 1;
            int resourceId = 1;

            DateOnly startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            DateOnly endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            decimal totalPrice = 100;

            // Act
            Guest guest = new Guest(guestId, "Allan", "Allansen", 12345678, "aa@aa.dk", "Danmark", "Dansk", "Allanvej 11");
            Resource resource = new Resource(resourceId, "Paradis", "Hytte", 500);

            // Assert
            Assert.Throws<Exception>(() => new Booking(guest.Id, resource.Id, guest.FirstName + guest.LastName,
                startDate, endDate, totalPrice));
        }
    }
}



