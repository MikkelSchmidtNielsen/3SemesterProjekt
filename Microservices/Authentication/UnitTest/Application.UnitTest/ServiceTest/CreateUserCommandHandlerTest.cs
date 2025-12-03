using Application.ApplicationDto;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Command;
using Application.Services.Command;
using Common;
using Common.ResultInterfaces;
using Domain.Models;
using Moq;

namespace UnitTest.Application.UnitTest.ServiceTest
{
    public class CreateUserCommandHandlerTest
    {
        [Fact]
        public async Task Handle_ReturnsSuccess_WhenAllStepsSucced()
        {
            // Arrange
            string email = "test@system.dk";
            string jwt = "jwt";

            Mock<IUserRepository> repo = new Mock<IUserRepository>();
            Mock<ICreateTokenCommandHandler> tokenHandler = new Mock<ICreateTokenCommandHandler>();

            User user = new User(email);

            // Mock repo
            repo
                .Setup(repo => repo.CreateUserAsync(It.Is<User>(user => user.Email == email)))
                .ReturnsAsync(Result<User>.Success(new User(email)));

            // Mock token creation
            tokenHandler
                .Setup(handler => handler.Handle(It.Is<User>(user => user.Email == email)))
                .Returns(Result<string>.Success(jwt));

            CreateUserCommandHandler sut = new CreateUserCommandHandler(repo.Object, tokenHandler.Object);

            // Act
            IResult<CreateUserResponseDto> result = await sut.HandleAsync(email);

            // Assert
            Assert.True(result.IsSuccess());

            CreateUserResponseDto success = result.GetSuccess().OriginalType;
            Assert.Equal(email, success.Email);
            Assert.Equal(jwt, success.Token);

            // Verify mocks were called
            repo.Verify(repo => repo.CreateUserAsync(It.Is<User>(u => u.Email == email)), Times.Once);
            tokenHandler.Verify(handler => handler.Handle(It.Is<User>(u => u.Email == email)), Times.Once);
        }

        [Fact]
        public async Task Handle_ReturnsError_WhenRepositoryFails()
        {
            // Arrange
            string email = "test@system.dk";

            Mock<IUserRepository> repo = new Mock<IUserRepository>();
            Mock<ICreateTokenCommandHandler> tokenHandler = new Mock<ICreateTokenCommandHandler>();

            User user = new User(email);

            Exception ex = new Exception("Token creation failed");

            // Mock repo
            repo
                .Setup(repo => repo.CreateUserAsync(It.Is<User>(user => user.Email == email)))
                .ReturnsAsync(Result<User>.Error(new User(email), ex));

            CreateUserCommandHandler sut = new CreateUserCommandHandler(repo.Object, tokenHandler.Object);

            // Act
            IResult<CreateUserResponseDto> result = await sut.HandleAsync(email);

            // Assert
            Assert.True(result.IsError());
            IResultError<CreateUserResponseDto> error = result.GetError();
            Assert.Equal(ex, error.Exception);

            // Verify mock were called
            repo.Verify(repo => repo.CreateUserAsync(It.IsAny<User>()), Times.Once);

            // Verify mock weren't called
            tokenHandler.Verify(handler => handler.Handle(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ReturnsError_WhenTokenCreationFails()
        {
            // Arrange
            string email = "test@system.dk";
            string jwt = "jwt";

            Mock<IUserRepository> repo = new Mock<IUserRepository>();
            Mock<ICreateTokenCommandHandler> tokenHandler = new Mock<ICreateTokenCommandHandler>();

            User user = new User(email);

            Exception ex = new Exception("Database error");

            // Mock repo
            repo
                .Setup(repo => repo.CreateUserAsync(It.Is<User>(user => user.Email == email)))
                .ReturnsAsync(Result<User>.Success(new User(email)));

            // Mock token creation
            tokenHandler
                .Setup(handler => handler.Handle(It.Is<User>(user => user.Email == email)))
                .Returns(Result<string>.Error(jwt, ex));

            CreateUserCommandHandler sut = new CreateUserCommandHandler(repo.Object, tokenHandler.Object);

            // Act
            IResult<CreateUserResponseDto> result = await sut.HandleAsync(email);

            // Assert
            Assert.True(result.IsError());
            IResultError<CreateUserResponseDto> error = result.GetError();
            Assert.Equal(ex, error.Exception);

            // Verify mock were called
            repo.Verify(repo => repo.CreateUserAsync(It.IsAny<User>()), Times.Once);
            tokenHandler.Verify(handler => handler.Handle(It.IsAny<User>()), Times.Once);
        }
    }
}
