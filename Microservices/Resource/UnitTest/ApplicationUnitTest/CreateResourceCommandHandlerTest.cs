using Application.ApplicationDto;
using Application.RepositoryInterfaces;
using Application.Services.Command;
using Common;
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

namespace UnitTest.ApplicationUnitTest
{
	public class CreateResourceCommandHandlerTest
	{
		[Fact]
		public async Task HandleAsync_ShouldCallRepository_WhenFactoryDoesNotThrowException()
		{
			// Arrange
			Resource factoryRetunResource = 
				Impression.Of<Resource>()
				.Randomize()
				.Create();

			Mock<IResourceFactory> mockFactory = new Mock<IResourceFactory>();
			mockFactory
				.Setup(fc => fc.CreateAsync(It.IsAny<CreateResourceFactoryDto>()))
				.ReturnsAsync(factoryRetunResource);

			Mock<IResourceRepository> mockRepository = new Mock<IResourceRepository>();
			mockRepository
				.Setup(repo => repo.CreateAsync(factoryRetunResource))
				.ReturnsAsync(Result<Resource>.Success(factoryRetunResource));

			CreateResourceCommandHandler sut = 
				Impression.Of<CreateResourceCommandHandler>()
				.With("_factory", mockFactory.Object)
				.With("_repository", mockRepository.Object)
				.Create();

			CreateResourceCommandDto dto = Impression.Of<CreateResourceCommandDto>().Randomize().Create();

			// Act
			await sut.HandleAsync(dto);

			// Assert
			mockRepository.Verify(repo => repo.CreateAsync(factoryRetunResource), Times.Once());
		}

		[Fact]
		public async Task HandleAsync_ShouldReturnSuccess_WhenBothFactoryAndRepositoryHasBeenCalled()
		{
			// Arrange
			Resource factoryRetunResource = 
				Impression.Of<Resource>()
				.Randomize()
				.Create();

			Mock<IResourceFactory> mockFactory = new Mock<IResourceFactory>();
			mockFactory
				.Setup(fc => fc.CreateAsync(It.IsAny<CreateResourceFactoryDto>()))
				.ReturnsAsync(factoryRetunResource);

			Mock<IResourceRepository> mockRepository = new Mock<IResourceRepository>();
			mockRepository
				.Setup(repo => repo.CreateAsync(factoryRetunResource))
				.ReturnsAsync(Result<Resource>.Success(factoryRetunResource));

			CreateResourceCommandHandler sut =
				Impression.Of<CreateResourceCommandHandler>()
				.With("_factory", mockFactory.Object)
				.With("_repository", mockRepository.Object)
				.Create();

			CreateResourceCommandDto dto = Impression.Of<CreateResourceCommandDto>().Randomize().Create();	
			// Act
			await sut.HandleAsync(dto);

			// Assert
			mockFactory.Verify(fc => fc.CreateAsync(It.IsAny<CreateResourceFactoryDto>()), Times.Once());
			mockRepository.Verify(repo => repo.CreateAsync(factoryRetunResource), Times.Once());
		}
	}
}
