using Application.ApplicationDto.Command;
using Application.InfrastructureDto;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Query;
using Application.Services.Command;
using Common;
using Common.ResultInterfaces;
using Domain.DomainInterfaces;
using Domain.Models;
using Domain.ModelsDto;
using Moq;

namespace UnitTest.Application.UnitTest.ServiceTest
{
    public class GuestCreateCommandTest
    {
		[Fact]
		public async Task CreateGuestAsync_ReturnsSuccess_WhenFactoryAndRepositorySucceed()
		{
			// Arrange
			Mock<IGuestFactory> guestFactory = new Mock<IGuestFactory>();
			Mock<IGuestRepository> repo = new Mock<IGuestRepository>();
			Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
			Mock<IUserAuthenticationApiService> api = new Mock<IUserAuthenticationApiService>();

			GuestCreateRequestDto createDto = new GuestCreateRequestDto();
			Guest guest = new Guest("Mikkel", null, null, "Mikkel@rosendahllarsen.dk", null, null, null);
			Exception repoException = new Exception("Database error");
			CreateUserByApiReponseDto apiDto = new CreateUserByApiReponseDto() { JwtToken = Guid.NewGuid().ToString() };


			guestFactory
				.Setup(guestFactory => guestFactory.CreateAsync(It.IsAny<CreatedGuestDto>()))
				.ReturnsAsync(Result<Guest>.Success(guest));

			repo
				.Setup(repo => repo.CreateGuestAsync(guest))
				.ReturnsAsync(Result<Guest>.Success(guest));

			api
				.Setup(api => api.RegisterUserAsync(It.IsAny<string>()))
				.ReturnsAsync(Result<CreateUserByApiReponseDto>.Success(apiDto));

			GuestCreateCommand sut = new GuestCreateCommand(guestFactory.Object, repo.Object, uow.Object, api.Object);

			// Act
			IResult<Guest> result = await sut.CreateGuestAsync(createDto);

			// Assert
			Assert.True(result.IsSucces());
			IResultSuccess<Guest> success = result.GetSuccess();
			Assert.Equal(guest, success.OriginalType);

			guestFactory.Verify(x => x.CreateAsync(It.IsAny<CreatedGuestDto>()), Times.Once);

			repo.Verify(x => x.CreateGuestAsync(guest), Times.Once);
		}

		[Fact]
		public async Task CreateGuestAsync_ReturnsError_WhenRepositoryCreateFails()
		{
			// Arrange
			Mock<IGuestFactory> guestFactory = new Mock<IGuestFactory>();
			Mock<IGuestRepository> repo = new Mock<IGuestRepository>();
			Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
			Mock<IUserAuthenticationApiService> api = new Mock<IUserAuthenticationApiService>();

			GuestCreateRequestDto createDto = new GuestCreateRequestDto();
			Guest guest = new Guest("Mikkel", null, null, null, null, null, null);
			Exception repoException = new Exception("Database error");
			CreateUserByApiReponseDto apiDto = new CreateUserByApiReponseDto() { JwtToken = Guid.NewGuid().ToString() };

			guestFactory
				.Setup(x => x.CreateAsync(It.IsAny<CreatedGuestDto>()))
				.ReturnsAsync(Result<Guest>.Success(guest));

			repo
				.Setup(x => x.CreateGuestAsync(guest))
				.ReturnsAsync(Result<Guest>.Error(guest, repoException));

			api
				.Setup(api => api.RegisterUserAsync(It.IsAny<string>()))
				.ReturnsAsync(Result<CreateUserByApiReponseDto>.Success(apiDto));

			GuestCreateCommand sut = new GuestCreateCommand(guestFactory.Object, repo.Object, uow.Object, api.Object);

			// Act
			IResult<Guest> result = await sut.CreateGuestAsync(createDto);

			// Assert
			Assert.True(result.IsError());
			IResultError<Guest> error = result.GetError();
			Assert.Equal(guest, error.OriginalType);
			Assert.Equal(repoException, error.Exception);

			guestFactory.Verify(x => x.CreateAsync(It.IsAny<CreatedGuestDto>()), Times.Once);
			repo.Verify(x => x.CreateGuestAsync(guest), Times.Once);
		}

		[Fact]
		public async Task CreateGuestAsync_ReturnsError_WhenApiCreateFails()
		{
			// Arrange
			Mock<IGuestFactory> guestFactory = new Mock<IGuestFactory>();
			Mock<IGuestRepository> repo = new Mock<IGuestRepository>();
			Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
			Mock<IUserAuthenticationApiService> api = new Mock<IUserAuthenticationApiService>();

			GuestCreateRequestDto createDto = new GuestCreateRequestDto();
			Guest guest = new Guest(firstName: "Mikkel", null, null, email: "test@test.dk", null, null, null);
			Exception apiException = new Exception("Api error");
			CreateUserByApiReponseDto apiDto = new CreateUserByApiReponseDto() { JwtToken = Guid.NewGuid().ToString() };

			guestFactory
				.Setup(x => x.CreateAsync(It.IsAny<CreatedGuestDto>()))
				.ReturnsAsync(Result<Guest>.Success(guest));

			repo
				.Setup(x => x.CreateGuestAsync(guest))
				.ReturnsAsync(Result<Guest>.Success(guest));

			api
				.Setup(api => api.RegisterUserAsync(It.IsAny<string>()))
				.ReturnsAsync(Result<CreateUserByApiReponseDto>.Error(null, apiException));

			GuestCreateCommand sut = new GuestCreateCommand(guestFactory.Object, repo.Object, uow.Object, api.Object);

			// Act
			IResult<Guest> result = await sut.CreateGuestAsync(createDto);

			// Assert
			Assert.True(result.IsError());
			IResultError<Guest> error = result.GetError();
			Assert.Equal(guest, error.OriginalType);
			Assert.Equal(apiException, error.Exception);

			guestFactory.Verify(x => x.CreateAsync(It.IsAny<CreatedGuestDto>()), Times.Once);
			repo.Verify(x => x.CreateGuestAsync(guest), Times.Once);
		}
	}
}
