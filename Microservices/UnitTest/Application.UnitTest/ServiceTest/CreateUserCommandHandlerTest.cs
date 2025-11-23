using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Command;
using Application.Services.Command;
using Common;
using Common.ResultInterfaces;
using Domain;
using Domain.DomainInterfaces;
using Domain.ModelsDto;
using Moq;
using UnitTest.UnitTestHelpingTools;

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
            Mock<IUserFactory> factory = new Mock<IUserFactory>();
            Mock<ICreateTokenCommandHandler> tokenHandler = new Mock<ICreateTokenCommandHandler>();

            User user = Impression.Of<User>()
                .WithDefaults()
                .Create();

            // Mock factory creation
            factory
                .Setup(factory => factory.Create(It.IsAny<CreateUserResponseDto>()))
                .Returns(Result<User>.Success(user));

            // Mock repo
            repo
                .Setup(repo => repo.CreateUserAsync(user))
                .ReturnsAsync(Result<User>.Success(user));

            // Mock token creation
            tokenHandler
                .Setup(handler => handler.Handle(user))
                .Returns(Result<string>.Success(jwt));

            CreateUserCommandHandler sut = new CreateUserCommandHandler(repo.Object, factory.Object, tokenHandler.Object);

            // Act
            IResult<CreateUserResponseDto> result = await sut.Handle(email);

            // Assert
            Assert.True(result.IsSuccess());

            CreateUserResponseDto success = result.GetSuccess().OriginalType;
            Assert.Equal(email, success.Email);
            Assert.Equal(jwt, success.Token);

            // Verify mocks were called
            factory.Verify(factory => factory.Create(It.IsAny<CreateUserResponseDto>()), Times.Once);
            repo.Verify(repo => repo.CreateUserAsync(user), Times.Once);
            tokenHandler.Verify(handler => handler.Handle(user), Times.Once);
        }

        [Fact]
        public async Task Handle_ReturnsError_WhenUserCreationFails()
        {
            // Arrange
            string email = "test@system.dk";

            Mock<IUserRepository> repo = new Mock<IUserRepository>();
            Mock<IUserFactory> factory = new Mock<IUserFactory>();
            Mock<ICreateTokenCommandHandler> tokenHandler = new Mock<ICreateTokenCommandHandler>();

            Exception ex = new Exception("User creation failed");

            // Mock factory creation
            factory
                .Setup(factory => factory.Create(It.IsAny<CreateUserResponseDto>()))
                .Returns(Result<User>.Error(null!, ex));

            CreateUserCommandHandler sut = new CreateUserCommandHandler(repo.Object, factory.Object, tokenHandler.Object);

            // Act
            IResult<CreateUserResponseDto> result = await sut.Handle(email);

            // Assert
            Assert.True(result.IsError());
            IResultError<CreateUserResponseDto> error = result.GetError();
            Assert.Equal(ex, error.Exception);

            // Verify mock were called
            factory.Verify(factory => factory.Create(It.IsAny<CreateUserResponseDto>()), Times.Once);

            // Verify mock weren't called
            repo.Verify(repo => repo.CreateUserAsync(It.IsAny<User>()), Times.Never);
            tokenHandler.Verify(handler => handler.Handle(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ReturnsError_WhenRepositoryFails()
        {
            // Arrange
            string email = "test@system.dk";

            Mock<IUserRepository> repo = new Mock<IUserRepository>();
            Mock<IUserFactory> factory = new Mock<IUserFactory>();
            Mock<ICreateTokenCommandHandler> tokenHandler = new Mock<ICreateTokenCommandHandler>();

            User user = Impression.Of<User>()
                .WithDefaults()
                .Create();

            Exception ex = new Exception("Token creation failed");

            // Mock factory creation
            factory
                .Setup(factory => factory.Create(It.IsAny<CreateUserResponseDto>()))
                .Returns(Result<User>.Success(user));

            // Mock repo
            repo
                .Setup(repo => repo.CreateUserAsync(user))
                .ReturnsAsync(Result<User>.Error(user, ex));

            CreateUserCommandHandler sut = new CreateUserCommandHandler(repo.Object, factory.Object, tokenHandler.Object);

            // Act
            IResult<CreateUserResponseDto> result = await sut.Handle(email);

            // Assert
            Assert.True(result.IsError());
            IResultError<CreateUserResponseDto> error = result.GetError();
            Assert.Equal(ex, error.Exception);

            // Verify mock were called
            factory.Verify(factory => factory.Create(It.IsAny<CreateUserResponseDto>()), Times.Once);
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
            Mock<IUserFactory> factory = new Mock<IUserFactory>();
            Mock<ICreateTokenCommandHandler> tokenHandler = new Mock<ICreateTokenCommandHandler>();

            User user = Impression.Of<User>()
                .WithDefaults()
                .Create();

            Exception ex = new Exception("Database error");

            // Mock factory creation
            factory
                .Setup(factory => factory.Create(It.IsAny<CreateUserResponseDto>()))
                .Returns(Result<User>.Success(user));

            // Mock repo
            repo
                .Setup(repo => repo.CreateUserAsync(user))
                .ReturnsAsync(Result<User>.Success(user));

            // Mock token creation
            tokenHandler
                .Setup(handler => handler.Handle(user))
                .Returns(Result<string>.Error(jwt, ex));

            CreateUserCommandHandler sut = new CreateUserCommandHandler(repo.Object, factory.Object, tokenHandler.Object);

            // Act
            IResult<CreateUserResponseDto> result = await sut.Handle(email);

            // Assert
            Assert.True(result.IsError());
            IResultError<CreateUserResponseDto> error = result.GetError();
            Assert.Equal(ex, error.Exception);

            // Verify mock were called
            factory.Verify(factory => factory.Create(It.IsAny<CreateUserResponseDto>()), Times.Once);
            repo.Verify(repo => repo.CreateUserAsync(It.IsAny<User>()), Times.Once);
            tokenHandler.Verify(handler => handler.Handle(It.IsAny<User>()), Times.Once);
        }
    }
}
