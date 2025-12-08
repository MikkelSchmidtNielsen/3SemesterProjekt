using Application.ApplicationDto.Command;
using Application.InfrastructureDto;
using Application.ServiceInterfaces.Query;
using Application.Services.Command;
using Common;
using Common.ResultInterfaces;
using Infrastructure.InternalApiCalls.ResourceApi;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnitTest.UnitTestHelpingTools;

namespace UnitTest.Application.UnitTest.ServiceTest
{
	public class UpdateResourceCommandTest
	{
		/// <summary>
		/// Simulates Increment from Resource Api
		/// </summary>
		/// <param name="rowVersion"></param>
		/// <returns></returns>
		public byte[] IncrementRowVersion(byte[] rowVersion)
		{
			ulong current = BitConverter.ToUInt64(rowVersion, 0);
			return BitConverter.GetBytes(current + 1);
		}

		[Fact]
		public async Task HandleAsync_ShouldReturnSucces_WhenApiServiceReturnsSucces()
		{
			// Arrange
			byte[] rowVersion = new byte[8];

			UpdateResourceCommandDto sutDto =
				Impression.Of<UpdateResourceCommandDto>()
				.With("RowVersion", rowVersion)
				.Randomize()
				.Create();

			UpdateResourceByApiResponseDto apiResponseDto = Mapper.Map<UpdateResourceByApiResponseDto>(sutDto);
			apiResponseDto.RowVersion = IncrementRowVersion(apiResponseDto.RowVersion);

			Mock<IResourceApiService> mockApiService = new Mock<IResourceApiService>();
			mockApiService.Setup(
				repo => repo.UpdateAsync(It.IsAny<UpdateResourceCommandDto>()))
				.ReturnsAsync(Result<UpdateResourceByApiResponseDto>.Success(apiResponseDto));

			UpdateResourceCommand sut = 
				Impression.Of<UpdateResourceCommand>()
				.With("_apiService", mockApiService.Object)
				.Create();

			// Act
			IResult<UpdateResourceResponseDto> sutResponse = await sut.HandleAsync(sutDto);

			// Assert
			Assert.True(sutResponse.IsSucces());
			UpdateResourceResponseDto responseDto = sutResponse.GetSuccess().OriginalType;
			Assert.Equal(sutDto.Name, responseDto.Name);
			Assert.Equal(sutDto.Type, responseDto.Type);
			Assert.Equal(sutDto.BasePrice, responseDto.BasePrice);
			Assert.Equal(sutDto.Location, responseDto.Location);
			Assert.Equal(sutDto.IsAvailable, responseDto.IsAvailable);
			Assert.Equal(sutDto.Description, responseDto.Description);
			Assert.NotEqual(sutDto.RowVersion, responseDto.RowVersion);
		}

		[Fact]
		public async Task HandlerAsync_ShouldReturnError_WhenApiServiceReturnsSucces()
		{
			// Arrange
			byte[] rowVersion = new byte[8];

			UpdateResourceCommandDto sutDto =
				Impression.Of<UpdateResourceCommandDto>()
				.With("RowVersion", rowVersion)
				.Randomize()
				.Create();

			UpdateResourceByApiResponseDto apiResponseDto = Mapper.Map<UpdateResourceByApiResponseDto>(sutDto);
			apiResponseDto.RowVersion = IncrementRowVersion(apiResponseDto.RowVersion);

			Exception expectedException = new Exception("Uventet fejl fra API");

			Mock<IResourceApiService> mockApiService = new Mock<IResourceApiService>();
			mockApiService.Setup(
				repo => repo.UpdateAsync(It.IsAny<UpdateResourceCommandDto>()))
				.ReturnsAsync(Result<UpdateResourceByApiResponseDto>.Error(null, expectedException));

			UpdateResourceCommand sut =
				Impression.Of<UpdateResourceCommand>()
				.With("_apiService", mockApiService.Object)
				.Create();

			// Act
			IResult<UpdateResourceResponseDto> sutResponse = await sut.HandleAsync(sutDto);

			// Assert
			Assert.True(sutResponse.IsError());
			Assert.Null(sutResponse.GetError().OriginalType);
			Assert.Equal(expectedException.Message, sutResponse.GetError().Exception!.Message);
		}
	}
}
