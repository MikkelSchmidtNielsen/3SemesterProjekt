using Application.ApplicationDto;
using Application.RepositoryInterfaces;
using Application.Services.Query;
using Common;
using Common.CustomExceptions;
using Common.ResultInterfaces;
using Domain.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.UnitTestHelpingTools;

namespace UnitTest.ApplicationUnitTest
{
	public class ReadResourceByIdQueryHandlerTest
	{
		[Fact]
		public async Task HandleAsync_ShouldReturnSucces_WhenRepositoryAResource()
		{
			// Arrange 
			Resource returnResource =
				Impression.Of<Resource>()
				.Randomize()
				.Create();

			Mock<IResourceRepository> mockRepository = new Mock<IResourceRepository>();
			mockRepository
				.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
				.ReturnsAsync(Result<Resource>.Success(returnResource));

			ReadResourceByIdQueryHandler sut =
				Impression.Of<ReadResourceByIdQueryHandler>()
				.With("_resourceRepository", mockRepository.Object)
				.Create();

			// Act
			IResult<ResourceResponseDto> result = await sut.HandleAsync(returnResource.Id);

			// Assert
			Assert.True(result.IsSucces());

			mockRepository.Verify(repo => repo.GetByIdAsync(returnResource.Id), Times.Once());
		}

		[Fact]
		public async Task HandleAsync_ShouldThrowNotFoundException_WhenRepositoryReturnsNull()
		{
			// Arrange 
			Resource returnResource =
				Impression.Of<Resource>()
				.Randomize()
				.Create();

			Mock<IResourceRepository> mockRepository = new Mock<IResourceRepository>();
			mockRepository
				.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
				.ReturnsAsync(Result<Resource>.Success(null)!);

			ReadResourceByIdQueryHandler sut =
				Impression.Of<ReadResourceByIdQueryHandler>()
				.With("_resourceRepository", mockRepository.Object)
				.Create();

			NotFoundException expectedException = new NotFoundException("Der er ikke nogen Resources i databasen med det Id");

			// Act + Assert
			NotFoundException actualException =
				await Assert.ThrowsAsync<NotFoundException>(
					() => sut.HandleAsync(returnResource.Id));

			Assert.Equal(expectedException.Message, actualException.Message);
			mockRepository.Verify(repo => repo.GetByIdAsync(returnResource.Id), Times.Once());
		}
	}
}
