using Common.CustomExceptions;
using Domain.DomainDtos;
using Domain.Models;
using Domain.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.DomainUnitTest;
using UnitTest.UnitTestHelpingTools;

namespace UnitTest.Domain.UnitTest
{
    public class ResourceEntityTest
    {
        [Fact]
        public static void RessourceValidation_ShouldPass_WhenGivenValidInfo()
        {
            // Arrange
			decimal basePrice = 200m;

			CreateResourceFactoryDto dto =
				Impression.Of<CreateResourceFactoryDto>()
				.With("BasePrice", basePrice)
				.Randomize()
				.Create();

			// Act
			Resource createdResource = new Resource(dto);

			// Assert
			Assert.Equal(dto.Name, createdResource.Name);
			Assert.Equal(dto.Type, createdResource.Type);
			Assert.Equal(dto.BasePrice, createdResource.BasePrice);
			Assert.Equal(dto.Location, createdResource.Location);
			Assert.Equal(dto.Description, createdResource.Description);
			Assert.True(createdResource.IsAvailable);
		}
        [Fact]
        public static void RessourceValidation_ShouldFail_IfNameIsNull_Integration()
        {
			// Arrange
			string? name = null;
			decimal basePrice = 200m;

			ArgumentException expectedException = new ArgumentException("Resource name is null.");

			CreateResourceFactoryDto dto =
				Impression.Of<CreateResourceFactoryDto>()
				.With("BasePrice", basePrice)
				.With("Name", name)
				.Randomize()
				.Create();

			// Assert
			ArgumentException actualException = Assert.Throws<ArgumentException>(() => new Resource(dto));
        }
        [Fact]
        public static void RessourceValidation_ShouldFail_IfTypeIsNull_Integration()
        {
			// Arrange
			string? type = null;
			decimal basePrice = 200m;

			ArgumentException expectedException = new ArgumentException("Resource type is null.");

			CreateResourceFactoryDto dto =
				Impression.Of<CreateResourceFactoryDto>()
				.With("BasePrice", basePrice)
				.With("Type", type)
				.Randomize()
				.Create();

			// Assert
			ArgumentException actualException = Assert.Throws<ArgumentException>(() => new Resource(dto));
			Assert.Equal(expectedException.Message, actualException.Message);
		}

		[Fact]
		public static void ResourceUpdate_ShouldPass_WhenNewInformationIsValid()
		{
			// Arrange
			UpdateResourceDomainDto dto =
				Impression.Of<UpdateResourceDomainDto>()
				.Randomize()
				.Create();

			Resource resource =
				Impression.Of<Resource>()
				.Randomize()
				.Create();

			// Act
			resource.UpdateResource(dto);

			// Assert
			Assert.Equal(dto.Name, resource.Name);
			Assert.Equal(dto.Type, resource.Type);
			Assert.Equal(dto.BasePrice, resource.BasePrice);
			Assert.Equal(dto.Location, resource.Location);
			Assert.Equal(dto.Description, resource.Description);
			Assert.Equal(dto.IsAvailable, resource.IsAvailable);
		}
	}
}
