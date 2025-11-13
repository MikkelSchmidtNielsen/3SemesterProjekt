using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace UnitTest.Domain.UnitTest
{
    public class BookingEntityTest
    {
        [Fact]
        public void GuestBookingShouldRequireAGuest()
        {
            // Arrange
            

            // Act


            // Assert


        }
        [Fact]
        public void GuestBookingShouldRequireAResource()
        {
            // Arrange


            // Act


            // Assert
        }
        [Fact]
        public void GuestBookingShouldCreateWhenGivenCorrectInformation()
        {
            // Arrange
            int guestId = 1;

            int resourceId = 1;

            DateOnly startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            DateOnly endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(7));
            double totalPrice = 100;

            // Act
            Guest guest = new Guest(guestId, "Allan", "Allansen", 12345678, "aa@aa.dk", "Danmark", "Dansk", "Allanvej 11");
            Resource resource = new Resource(resourceId, "Hytte 4");
            Booking booking = new Booking(startDate, endDate, totalPrice, guest, resource);

            // Assert
            Assert.Equal(startDate, booking.StartDate);
            Assert.Equal(endDate, booking.EndDate);
            Assert.Equal(totalPrice, booking.TotalPrice);
            Assert.Equal(guest, booking.Guest);
            Assert.Equal(resource, booking.Resource);
        }
    }
}
