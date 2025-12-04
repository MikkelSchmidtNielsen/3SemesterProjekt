using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Command;

namespace UnitTest.Application.UnitTest.ServiceTest
{
    public class CreateOptCommandHandlerTest
    {
        [Fact]
        public void Handle_ShouldReturnSuccess_WhenOptIsSentToMail()
        {
            // Arrange
            string email = "test@system.dk";

            CreateOptCommandHandler sut = new CreateOptCommandHandler();

            // Act

            sut.Handle(email);

            // Assert
        }
    }
}
