using Application.ApplicationDto;
using Application.RepositoryInterfaces;
using Application.Services.Command;
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
	public class UpdateResourceByIdCommandHandlerTest
	{
		[Fact]
		public async Task HandleAsync_ShouldThrowNotFound_WhenGetByIdReturnsNull()
		{
			// Arrange
			Mock<IResourceRepository> mockRepository = new Mock<IResourceRepository>();
			mockRepository
				.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
				.ReturnsAsync(Result<Resource>.Success(null));

			UpdateResourceByIdCommandHandler sut =
				Impression.Of<UpdateResourceByIdCommandHandler>()
				.With("_resourceRepository", mockRepository.Object)
				.Create();

			int id = 1;
			UpdateResourceByIdCommandDto dto =
				Impression.Of<UpdateResourceByIdCommandDto>()
				.Randomize()
				.Create();

			NotFoundException expectedException = new NotFoundException("Der er ikke nogen Resources i databasen med det Id");

			// Act + Assert
			NotFoundException acutalException =
				await Assert.ThrowsAsync<NotFoundException>(
					() => sut.HandleAsync(id, dto));

			Assert.Equal(expectedException.Message, acutalException.Message);
		}

		[Fact]
		public async Task HandleAsync_ShouldReturnSucces_WhenRepositoryUpdateReturnsSucces()
		{
			// Arrange
			Resource repositoryResourceReturn =
				Impression.Of<Resource>()
				.Randomize()
				.Create();

			Mock<IResourceRepository> mockRepository = new Mock<IResourceRepository>();
			mockRepository
				.Setup(repo => repo.UpdateAsync(It.IsAny<Resource>()))
				.ReturnsAsync(Result<Resource>.Success(repositoryResourceReturn));
			mockRepository
				.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
				.ReturnsAsync(Result<Resource>.Success(repositoryResourceReturn));

			UpdateResourceByIdCommandHandler sut =
				Impression.Of<UpdateResourceByIdCommandHandler>()
				.With("_resourceRepository", mockRepository.Object)
				.Create();

			int id = 1;
			UpdateResourceByIdCommandDto dto =
				Impression.Of<UpdateResourceByIdCommandDto>()
				.Randomize()
				.Create();

			// Act
			IResult<ResourceResponseDto> response = await sut.HandleAsync(id, dto);

			ResourceResponseDto responseValue = response.GetSuccess().OriginalType;

			Assert.True(response.IsSucces());
			Assert.NotNull(responseValue);
			mockRepository.Verify(repo => repo.GetByIdAsync(id), Times.Once());
			mockRepository.Verify(repo => repo.UpdateAsync(repositoryResourceReturn), Times.Once());
		}
	}
}
