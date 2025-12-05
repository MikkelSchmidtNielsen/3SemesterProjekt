using Common;
using Common.CustomExceptions;
using Common.ResultInterfaces;
using Infrastructure.InternalApiCalls;
using Moq;
using Presentation.Client.Services.Implementation;
using Presentation.Shared.Models;
using Presentation.Shared.Refit;
using Refit;
using System.Net;
using System.Text;
using UnitTest.UnitTestHelpingTools;

namespace UnitTest.Presentation.UnitTest.ClientTest
{
	public class UpdateResourceServiceTest
	{
		// UPDATE
		[Fact]
		public async Task ReadAllResourceAsync_When200_ReturnsSuccessResult()
		{
			// Arrange
			UpdateResourceModel model =
				Impression.Of<UpdateResourceModel>()
				.Randomize()
				.Create();

			Mock<IUpdateResourceApi> apiMock = new Mock<IUpdateResourceApi>();
			apiMock.Setup(x => x.UpdateResource(It.IsAny<int>(), It.IsAny<UpdateResourceModel>()))
				.ReturnsAsync(Result<UpdateResourceModel>.Success(model));


			UpdateResourceService sut =
				Impression.Of<UpdateResourceService>()
				.With("_api", apiMock.Object)
				.Create();

			// Act
			IResult<UpdateResourceModel> result = await sut.UpdateResourceAsync(model);

			// Assert
			Assert.True(result.IsSucces());
			Assert.Equal(model, result.GetSuccess().OriginalType);
		}

		[Theory]
		[InlineData(HttpStatusCode.Conflict)]
		[InlineData(HttpStatusCode.NotFound)]
		[InlineData(HttpStatusCode.InternalServerError)]
		public async Task UpdateResourceAsync_WhenErrorCode_ReturnsErrorResult(HttpStatusCode statuscode)
		{
			// Arrange
			UpdateResourceModel model =
				Impression.Of<UpdateResourceModel>()
				.Randomize()
				.Create();

			string jsonError = "{\"message\":\"Default Error Message\"}";

			ApiException expectedError = await CreateApiException(statuscode, jsonError);

			Mock<IUpdateResourceApi> apiMock = new Mock<IUpdateResourceApi>();
			apiMock.Setup(x => x.UpdateResource(It.IsAny<int>(), It.IsAny<UpdateResourceModel>()))
				.ThrowsAsync(expectedError);


			UpdateResourceService sut =
				Impression.Of<UpdateResourceService>()
				.With("_api", apiMock.Object)
				.Create();

			// Act
			IResult<UpdateResourceModel> result = await sut.UpdateResourceAsync(model);

			// Assert
			Assert.False(result.IsSucces());
			ApiErrorException ex = Assert.IsType<ApiErrorException>(result.GetError().Exception);
			Assert.Equal((int)statuscode, ex.StatusCode);
			Assert.Equal("Default Error Message", ex.ApiErrorMessage);
		}


		 // READ ALL
		[Fact]
		public async Task ReadlAllResourceAsync_When200_ReturnsSuccessResult()
		{
			// Arrange
			UpdateResourceModel[] modelAsArray =
			{
				Impression.Of<UpdateResourceModel>()
				.Randomize()
				.Create()

			};

			Mock<IUpdateResourceApi> apiMock = new Mock<IUpdateResourceApi>();
			apiMock.Setup(x => x.GetAllResources())
				.ReturnsAsync(Result<IEnumerable<UpdateResourceModel>>.Success(modelAsArray));


			UpdateResourceService sut =
				Impression.Of<UpdateResourceService>()
				.With("_api", apiMock.Object)
				.Create();

			// Act
			IResult<IEnumerable<UpdateResourceModel>> result = await sut.GetAllResourcesAsync();

			// Assert
			Assert.True(result.IsSucces());
			Assert.Equal(modelAsArray, result.GetSuccess().OriginalType);
		}

		[Theory]
		[InlineData(HttpStatusCode.Conflict)]
		[InlineData(HttpStatusCode.NotFound)]
		[InlineData(HttpStatusCode.InternalServerError)]
		public async Task ReadlAllResourceAsync_WhenErrorCode_ReturnsErrorResult(HttpStatusCode statuscode)
		{
			// Arrange
			string jsonError = "{\"message\":\"Default Error Message\"}";

			ApiException expectedError = await CreateApiException(statuscode, jsonError);

			Mock<IUpdateResourceApi> apiMock = new Mock<IUpdateResourceApi>();
			apiMock.Setup(x => x.GetAllResources())
				.ThrowsAsync(expectedError);


			UpdateResourceService sut =
				Impression.Of<UpdateResourceService>()
				.With("_api", apiMock.Object)
				.Create();

			// Act
			IResult<IEnumerable<UpdateResourceModel>> result = await sut.GetAllResourcesAsync();

			// Assert
			Assert.False(result.IsSucces());
			ApiErrorException ex = Assert.IsType<ApiErrorException>(result.GetError().Exception);
			Assert.Equal((int)statuscode, ex.StatusCode);
			Assert.Equal("Default Error Message", ex.ApiErrorMessage);
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
