using Application.ApplicationDto;
using Application.Factories;
using Application.RepositoryInterfaces;
using Common;
using Common.CustomExceptions;
using Domain.DomainInterfaces;
using Domain.Models;
using Domain.ModelsDto;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.UnitTestHelpingTools;

namespace UnitTest.DomainUnitTest
{
	public class ResourceFactoryUnitTest
	{
		[Fact]
		public async Task CreateAsync_ShouldThrowConflicException_WhenNameIsAlreadyInDatabase()
		{
			// Arrange 
			Resource elementForArray = Impression.Of<Resource>().Randomize().Create();

			Resource[] firstGetAllResourceReturn = { elementForArray };

			// Arrange 
			Mock<IResourceRepository> mockRepository = new Mock<IResourceRepository>();
			mockRepository
				.Setup(repo => repo.GetAllAsync(It.Is<ReadResourceListQueryDto>(x => x.Name != null)))
				.ReturnsAsync(Result<IEnumerable<Resource>>.Success(firstGetAllResourceReturn));

			ResourceFactory sut =
				Impression.Of<ResourceFactory>()
				.With("_repository", mockRepository.Object)
				.Create();

			ConflictException expectedException = new ConflictException("Der eksistere allerede en resource med det navn");

			CreateResourceFactoryDto dto = Impression.Of<CreateResourceFactoryDto>().Randomize().Create();

			// Act + Assert
			ConflictException actualException = 
				await Assert.ThrowsAsync<ConflictException>(
				()=> sut.CreateAsync(dto));

			Assert.Equal(expectedException.Message, actualException.Message);
		}

		[Fact]
		public async Task CreateAsync_ShouldThrowConflictException_WhenLocationIsAlreadyInDatabase()
		{
			// Arrange 
			Resource elementForArray = Impression.Of<Resource>().Randomize().Create();

			Resource[] firstGetAllResourceReturn = { elementForArray };

			Mock<IResourceRepository> mockRepository = new Mock<IResourceRepository>();
			mockRepository
				.Setup(repo => repo.GetAllAsync(It.Is<ReadResourceListQueryDto>(x => x.Name != null)))
				.ReturnsAsync(Result<IEnumerable<Resource>>.Success(Array.Empty<Resource>()));

			mockRepository
				.Setup(repo => repo.GetAllAsync(It.Is<ReadResourceListQueryDto>(x => x.Location != null || x.Location == 0)))
				.ReturnsAsync(Result<IEnumerable<Resource>>.Success(firstGetAllResourceReturn));

			ResourceFactory sut =
				Impression.Of<ResourceFactory>()
				.With("_repository", mockRepository.Object)
				.Create();

			ConflictException expectedException = new ConflictException("Der eksistere allerede en resource på den lokalation");

			CreateResourceFactoryDto dto = 
				Impression.Of<CreateResourceFactoryDto>()
				.With("Location", null)
				.Randomize()
				.Create();

			// Act + Assert
			ConflictException actualException =
				await Assert.ThrowsAsync<ConflictException>(
				() => sut.CreateAsync(dto));

			Assert.Equal(expectedException.Message, actualException.Message);
		}

		[Fact]
		public async Task CreateAsync_ShouldReturnSucces_WhenNeitherNameOrLocationIsInDatabase()
		{
			// Arrange 
			Mock<IResourceRepository> mockRepository = new Mock<IResourceRepository>();
			mockRepository
				.Setup(repo => repo.GetAllAsync(It.Is<ReadResourceListQueryDto>(x => x.Name != null)))
				.ReturnsAsync(Result<IEnumerable<Resource>>.Success(Array.Empty<Resource>()));

			mockRepository
				.Setup(repo => repo.GetAllAsync(It.Is<ReadResourceListQueryDto>(x => x.Location != null || x.Location == 0)))
				.ReturnsAsync(Result<IEnumerable<Resource>>.Success(Array.Empty<Resource>()));

			ResourceFactory sut =
				Impression.Of<ResourceFactory>()
				.With("_repository", mockRepository.Object)
				.Create();

			decimal basePrice = 200m;

			CreateResourceFactoryDto dto =
				Impression.Of<CreateResourceFactoryDto>()
				.With("BasePrice", basePrice)
				.Randomize()
				.Create();

			// Act
			Resource createdResource = await sut.CreateAsync(dto);


			Assert.Equal(dto.Name, createdResource.Name);
			Assert.Equal(dto.Type, createdResource.Type);
			Assert.Equal(dto.BasePrice, createdResource.BasePrice);
			Assert.Equal(dto.Location, createdResource.Location);
			Assert.Equal(dto.Description, createdResource.Description);
			Assert.True(createdResource.IsAvailable);
		}
	}
}
