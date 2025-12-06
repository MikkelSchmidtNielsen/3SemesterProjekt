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
	public class ReadResourceWithCriteriaQueryHandlerTest
	{
		[Fact]
		public async Task HandleAsync_ShouldReturnSucces_WhenRepositoryReturnsNotEmpty()
		{
			// Arrange 
			Resource elementForArray = 
				Impression.Of<Resource>()
				.Randomize()
				.Create();

			ReadResourceListQueryDto criteria = 
				Impression.Of< ReadResourceListQueryDto>()
				.Randomize()
				.Create();

			Resource[] firstGetAllResourceReturn = { elementForArray };

			Mock<IResourceRepository> mockRepository = new Mock<IResourceRepository>();
			mockRepository
				.Setup(repo => repo.GetAllAsync(It.IsAny<ReadResourceListQueryDto>()))
				.ReturnsAsync(Result<IEnumerable<Resource>>.Success(firstGetAllResourceReturn));

			ReadResourceWithCriteriaQueryHandler sut = 
				Impression.Of<ReadResourceWithCriteriaQueryHandler>()
				.With("_resourceRepository", mockRepository.Object)
				.Create();

			// Act
			IResult<ICollection<ResourceResponseDto>> result = await sut.HandleAsync(criteria);

			// Assert
			Assert.True(result.IsSucces());
			Assert.True(result.GetSuccess().OriginalType.Count == 1);

			mockRepository.Verify(repo => repo.GetAllAsync(criteria), Times.Once());
		}

		[Fact]
		public async Task HandleAsync_ShouldThrowNotFoundException_WhenRepositoryReturnsEmpty()
		{
			// Arrange 
			ReadResourceListQueryDto criteria =
				Impression.Of<ReadResourceListQueryDto>()
				.Randomize()
				.Create();

			Mock<IResourceRepository> mockRepository = new Mock<IResourceRepository>();
			mockRepository
				.Setup(repo => repo.GetAllAsync(It.IsAny<ReadResourceListQueryDto>()))
				.ReturnsAsync(Result<IEnumerable<Resource>>.Success(Array.Empty<Resource>()));

			ReadResourceWithCriteriaQueryHandler sut =
				Impression.Of<ReadResourceWithCriteriaQueryHandler>()
				.With("_resourceRepository", mockRepository.Object)
				.Create();

			NotFoundException expectedException = new NotFoundException("Der er ikke nogen Resources i databasen med de kriteria");

			// Act + Assert
			NotFoundException actualException =
				await Assert.ThrowsAsync<NotFoundException>(
					() => sut.HandleAsync(criteria));

			Assert.Equal(expectedException.Message, actualException.Message);

			mockRepository.Verify(repo => repo.GetAllAsync(criteria), Times.Once());
		}
	}
}
