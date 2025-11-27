using Application.Factories;
using Common;
using Common.ResultInterfaces;
using Domain.Models;
using Domain.ModelsDto;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.Application.UnitTest.ServiceTest;

namespace UnitTest.Application.UnitTest.FactoryTest
{
    public class BookingFactoryTest
    {
		[Fact]
		public void FactoryCreation_ShouldReturnSuccess_WhenGivenCorrectBooking()
		{
			// Arrange
			Guest guestForTest = new Guest(firstName: "FirstName", lastName: "LastName", phoneNumber: 25252525, email: "Email@email.dk", country: "Country", language: "Language", address: "Address") 
			{ Id = 1}; // This is an "object initializer" used to set the guest's id when testing, even if the constructor does not
					   // accept id. Since the id is init: Init means "settable during initialization, which includes inside a
					   // constructor or in an object initializer.

			Resource resourceForTest = new Resource("Familiehytten", "Hytte", 400, 1, null) { Id = 1 };

			BookingFactory factory = new BookingFactory();
			CreateBookingFactoryDto dto = new CreateBookingFactoryDto
			{
                Guest = guestForTest,
				Resource = resourceForTest,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
				EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
				TotalPrice = 100,
			};

            // Act
            IResult<Booking> result = factory.Create(dto);
			IResultSuccess<Booking> succes = result.GetSuccess();
            
			// Assert
			Assert.True(result.IsSucces());
			//Assert.Equal(dto.Guest.Id, succes.OriginalType.Guest.Id);
			//Assert.Equal(dto.ResourceId, succes.OriginalType.ResourceId);
			//Assert.Equal(dto.StartDate, succes.OriginalType.StartDate);
			//Assert.Equal(dto.EndDate, succes.OriginalType.EndDate);
			//Assert.Equal(dto.TotalPrice, succes.OriginalType.TotalPrice);   
		}
    }
}
