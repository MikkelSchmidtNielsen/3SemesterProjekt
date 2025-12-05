using Application.ApplicationDto.Command;
using Application.InfrastructureDto;
using Common.CustomExceptions;
using Common.ResultInterfaces;
using Infrastructure.InternalApiCalls;
using Infrastructure.InternalApiCalls.ResourceApi;
using Moq;
using Refit;
using System.Net;
using System.Text;
using UnitTest.UnitTestHelpingTools;

namespace UnitTest.Infrastructure.UnitTest
{
    public class ResourceApiServiceTest
    {
        [Fact]
        public async Task CreateResourceAsync_When201_ReturnsSuccessResult()
        {
            // Arrange
            CreateResourceCommandDto command = Impression.Of<CreateResourceCommandDto>()
                .WithDefaults()
                .Create();

            CreateResourceByApiResponseDto responseDto = Impression.Of<CreateResourceByApiResponseDto>()
                .WithDefaults()
                .Create();

            Mock<IResourceApi> apiMock = new Mock<IResourceApi>();

            apiMock
                .Setup(x => x.CreateResourceAsync(It.IsAny<CreateResourceCommandDto>()))
                .ReturnsAsync(responseDto);

            ResourceApiService service = new ResourceApiService(apiMock.Object);

            // Act
            IResult<CreateResourceByApiResponseDto> result = await service.CreateResourceAsync(command);

            // Assert
            Assert.True(result.IsSucces());
            Assert.Equal(responseDto, result.GetSuccess().OriginalType);
        }

        [Fact]
        public async Task CreateResourceAsync_When409_ReturnsErrorResultWithApiError()
        {
            // Arrange
            CreateResourceCommandDto command = Impression.Of<CreateResourceCommandDto>()
                .WithDefaults()
                .Create();

            string jsonError = "{\"message\":\"Resource spot is occupied\"}";

            ApiException apiException = await CreateApiException(HttpStatusCode.Conflict, jsonError);

            Mock<IResourceApi> apiMock = new Mock<IResourceApi>();

            apiMock.Setup(x => x.CreateResourceAsync(It.IsAny<CreateResourceCommandDto>()))
                   .ThrowsAsync(apiException);

            ResourceApiService service = new ResourceApiService(apiMock.Object);

            // Act
            IResult<CreateResourceByApiResponseDto> result = await service.CreateResourceAsync(command);

            // Assert
            Assert.False(result.IsSucces());
            ApiErrorException ex = Assert.IsType<ApiErrorException>(result.GetError().Exception);
            Assert.Equal(409, ex.StatusCode);
            Assert.Equal("Resource spot is occupied", ex.ApiErrorMessage);
        }

        [Fact]
        public async Task CreateResourceAsync_When500_ReturnsErrorResultWithApiError()
        {
            // Arrange
            CreateResourceCommandDto command = Impression.Of<CreateResourceCommandDto>()
                .WithDefaults()
                .Create();

            string jsonError = "{\"message\":\"Resource spot is occupied\"}";

            ApiException apiException = await CreateApiException(HttpStatusCode.InternalServerError, jsonError);

            Mock<IResourceApi> apiMock = new Mock<IResourceApi>();
            apiMock.Setup(x => x.CreateResourceAsync(It.IsAny<CreateResourceCommandDto>()))
                   .ThrowsAsync(apiException);

            ResourceApiService service = new ResourceApiService(apiMock.Object);

            // Act
            IResult<CreateResourceByApiResponseDto> result = await service.CreateResourceAsync(command);

            // Assert
            Assert.True(result.IsError());
            ApiErrorException ex = Assert.IsType<ApiErrorException>(result.GetError().Exception);
            Assert.Equal(500, ex.StatusCode);
            Assert.Equal("Resource spot is occupied", ex.ApiErrorMessage);
        }

        [Fact]
        public async Task CreateResourceAsync_WhenBodyIsNotJson_StillReturnsApiErrorException()
        {
            // Arrange
            CreateResourceCommandDto command = Impression.Of<CreateResourceCommandDto>()
                .WithDefaults()
                .Create();

            string jsonError = "<html>not json</html>";

            ApiException apiException = await CreateApiException(HttpStatusCode.Conflict, jsonError);

            Mock<IResourceApi> apiMock = new Mock<IResourceApi>();
            apiMock.Setup(x => x.CreateResourceAsync(It.IsAny<CreateResourceCommandDto>()))
                   .ThrowsAsync(apiException);

            ResourceApiService service = new ResourceApiService(apiMock.Object);

            // Act
            IResult<CreateResourceByApiResponseDto> result = await service.CreateResourceAsync(command);


            // Assert
            Assert.True(result.IsError());
            ApiErrorException ex = Assert.IsType<ApiErrorException>(result.GetError().Exception);
            Assert.Equal("Unexpected error format from API", ex.ApiErrorMessage);
            Assert.Equal(409, ex.StatusCode);
        }

        [Fact]
        public async Task CreateResourceAsync_WhenBodyIsEmpty_StillCreatesApiErrorException()
        {
            // Arrange
            CreateResourceCommandDto command = Impression.Of<CreateResourceCommandDto>()
                .WithDefaults()
                .Create();

            string jsonError = "";

            ApiException apiException = await CreateApiException(HttpStatusCode.Conflict, jsonError);

            Mock<IResourceApi> apiMock = new Mock<IResourceApi>();
            apiMock.Setup(x => x.CreateResourceAsync(It.IsAny<CreateResourceCommandDto>()))
                   .ThrowsAsync(apiException);

            ResourceApiService service = new ResourceApiService(apiMock.Object);

            // Act
            IResult<CreateResourceByApiResponseDto> result = await service.CreateResourceAsync(command);

            // Assert
            Assert.True(result.IsError());
            ApiErrorException ex = Assert.IsType<ApiErrorException>(result.GetError().Exception);
            Assert.Null(ex.ApiErrorMessage);
            Assert.Equal(409, ex.StatusCode);
        }

        [Fact]
        public async Task CreateResourceAsync_WhenHttpRequestException_ReturnsGeneralError()
        {
            // HttpRequestException is when network is down or Api cant be reached
            // Arrange
            CreateResourceCommandDto command = Impression.Of<CreateResourceCommandDto>()
                .WithDefaults()
                .Create();

            Mock<IResourceApi> apiMock = new Mock<IResourceApi>();

            apiMock.Setup(x => x.CreateResourceAsync(It.IsAny<CreateResourceCommandDto>()))
                   .ThrowsAsync(new HttpRequestException("Network down"));

            ResourceApiService service = new ResourceApiService(apiMock.Object);

            // Act
            IResult<CreateResourceByApiResponseDto> result = await service.CreateResourceAsync(command);

            // Assert
            Assert.True(result.IsError());
            Assert.IsType<HttpRequestException>(result.GetError().Exception);
            Assert.Equal("Network down", result.GetError().Exception!.Message);
        }

        [Fact]
        public async Task CreateResourceAsync_WhenUnexpectedException_ReturnsErrorResult()
        {
            // Arrange
            CreateResourceCommandDto command = Impression.Of<CreateResourceCommandDto>()
                .WithDefaults()
                .Create();

            Mock<IResourceApi> apiMock = new Mock<IResourceApi>();

            apiMock.Setup(x => x.CreateResourceAsync(It.IsAny<CreateResourceCommandDto>()))
                   .ThrowsAsync(new Exception("Something bad"));

            ResourceApiService service = new ResourceApiService(apiMock.Object);

            // Act
            IResult<CreateResourceByApiResponseDto> result = await service.CreateResourceAsync(command);

            // Assert
            Assert.True(result.IsError());
            Assert.Equal("Something bad", result.GetError().Exception!.Message);
        }

        // Helper Method
        public async Task<ApiException> CreateApiException(HttpStatusCode code, string body)
        {
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
