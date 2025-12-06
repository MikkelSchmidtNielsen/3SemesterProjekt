using global::Infrastructure.InternalApiCalls.UserAuthenticationApi;
using Infrastructure.InternalApiCalls;
using Moq;
using Refit;
using System.Net;
using System.Text;

namespace UnitTest.Infrastructure.UnitTest
{
	public class UserAuthenticationApiServiceTests
	{
		[Fact]
		public async Task RegisterUserAsync_When201_ReturnsSuccessResult()
		{
			// Arrange
			var apiMock = new Mock<IUserAuthenticationApi>();
			apiMock.Setup(x => x.RegisterUserAsync(It.IsAny<string>()))
				   .ReturnsAsync("jwt-token-123");

			var service = new UserAuthenticationApiService(apiMock.Object);

			// Act
			var result = await service.RegisterUserAsync("test@mail.com");

			// Assert
			Assert.True(result.IsSucces());
			Assert.Equal("jwt-token-123", result.GetSuccess().OriginalType.JwtToken);
		}

		[Fact]
		public async Task RegisterUserAsync_When409_ReturnsErrorResultWithApiError()
		{
			// Arrange
			var apiException = await CreateApiException(
				HttpStatusCode.Conflict,
				"{\"message\":\"Email already exists\"}"
			);

			var apiMock = new Mock<IUserAuthenticationApi>();
			apiMock.Setup(x => x.RegisterUserAsync(It.IsAny<string>()))
				   .ThrowsAsync(apiException);

			var service = new UserAuthenticationApiService(apiMock.Object);

			// Act
			var result = await service.RegisterUserAsync("test@mail.com");

			// Assert
			Assert.False(result.IsSucces());
			var ex = Assert.IsType<ApiErrorException>(result.GetError().Exception);
			Assert.Equal(409, ex.StatusCode);
			Assert.Equal("Email already exists", ex.ApiErrorMessage);
		}

		[Fact]
		public async Task RegisterUserAsync_When500_ReturnsErrorResultWithApiError()
		{
			// Arrange
			var apiException = await CreateApiException(
				HttpStatusCode.InternalServerError,
				"{\"message\":\"Something went wrong\"}"
			);

			var apiMock = new Mock<IUserAuthenticationApi>();
			apiMock.Setup(x => x.RegisterUserAsync(It.IsAny<string>()))
				   .ThrowsAsync(apiException);

			var service = new UserAuthenticationApiService(apiMock.Object);

			// Act
			var result = await service.RegisterUserAsync("test@mail.com");

			// Assert
			Assert.True(result.IsError());
			var ex = Assert.IsType<ApiErrorException>(result.GetError().Exception);
			Assert.Equal(500, ex.StatusCode);
			Assert.Equal("Something went wrong", ex.ApiErrorMessage);
		}

		[Fact]
		public async Task RegisterUserAsync_WhenBodyIsNotJson_StillReturnsApiErrorException()
		{
			// Arrange
			var apiException = await CreateApiException(
				HttpStatusCode.Conflict,
				"<html>not json</html>"
			);

			var apiMock = new Mock<IUserAuthenticationApi>();
			apiMock.Setup(x => x.RegisterUserAsync(It.IsAny<string>()))
				   .ThrowsAsync(apiException);

			var service = new UserAuthenticationApiService(apiMock.Object);

			// Act
			var result = await service.RegisterUserAsync("test");


			// Assert
			Assert.True(result.IsError());
			var ex = Assert.IsType<ApiErrorException>(result.GetError().Exception);
			Assert.Equal("Uventet fejl fra API", ex.ApiErrorMessage);
			Assert.Equal(409, ex.StatusCode);
		}

		[Fact]
		public async Task RegisterUserAsync_WhenBodyIsEmpty_StillCreatesApiErrorException()
		{
			// Arrange
			var apiException = await CreateApiException(
				HttpStatusCode.Conflict,
				""
			);

			var apiMock = new Mock<IUserAuthenticationApi>();
			apiMock.Setup(x => x.RegisterUserAsync(It.IsAny<string>()))
				   .ThrowsAsync(apiException);

			var service = new UserAuthenticationApiService(apiMock.Object);

			// Act
			var result = await service.RegisterUserAsync("test");

			// Assert
			Assert.True(result.IsError());
			var ex = Assert.IsType<ApiErrorException>(result.GetError().Exception);
			Assert.Null(ex.ApiErrorMessage);
			Assert.Equal(409, ex.StatusCode);
		}

		[Fact]
		public async Task RegisterUserAsync_WhenHttpRequestException_ReturnsGeneralError()
		{
			// HttpRequestException is when network is down or Api cant be reached
			// Arrange
			var apiMock = new Mock<IUserAuthenticationApi>();
			apiMock.Setup(x => x.RegisterUserAsync(It.IsAny<string>()))
				   .ThrowsAsync(new HttpRequestException("Network down"));

			var service = new UserAuthenticationApiService(apiMock.Object);

			// Act
			var result = await service.RegisterUserAsync("test");

			// Assert
			Assert.True(result.IsError());
			Assert.IsType<HttpRequestException>(result.GetError().Exception);
			Assert.Equal("Network down", result.GetError().Exception!.Message);
		}

		[Fact]
		public async Task RegisterUserAsync_WhenUnexpectedException_ReturnsErrorResult()
		{
			// Arrange
			var apiMock = new Mock<IUserAuthenticationApi>();
			apiMock.Setup(x => x.RegisterUserAsync(It.IsAny<string>()))
				   .ThrowsAsync(new Exception("Something bad"));

			var service = new UserAuthenticationApiService(apiMock.Object);

			// Act
			var result = await service.RegisterUserAsync("test");

			// Assert
			Assert.True(result.IsError());
			Assert.Equal("Something bad", result.GetError().Exception!.Message);
		}

		private async Task<ApiException> CreateApiException(HttpStatusCode code, string body)
		{
			// Helping Method to create refits ApiExceptions, so I can test my try catches
			var response = new HttpResponseMessage(code)
			{
				Content = new StringContent(body ?? "", Encoding.UTF8, "application/json")
			};

			var request = new HttpRequestMessage(HttpMethod.Post, "http://test/api");
			var settings = new RefitSettings(); // required

			return await ApiException.Create(
				request,
				HttpMethod.Post,
				response,
				settings
			);
		}
	}
}
