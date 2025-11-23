using Application.Factories;
using Common.ResultInterfaces;
using Domain;
using Domain.ModelsDto;
using UnitTest.UnitTestHelpingTools;

namespace UnitTest.Application.UnitTest.FactoryTest
{
    public class UserFactoryTest
    {
        [Fact]
        public void FactoryCreation_ShouldReturnSuccess_WhenGivenCorrectUserInformation()
        {
            // Arrange
            UserFactory factory = new UserFactory();
            CreateUserResponseDto dto = Impression.Of<CreateUserResponseDto>()
                .With("Email", "test@mail.dk")
                .WithDefaults()
                .Create();

            // Act
            IResult<User> result = factory.Create(dto);
            IResultSuccess<User> success = result.GetSuccess();

            // Assert
            Assert.True(result.IsSuccess());
            Assert.Equal(dto.Email, success.OriginalType.Email);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Danline@dk")]
        [InlineData("DanlineDk")]
        [InlineData("@Danline.dk")]
        public void FactoryCreation_ShouldThrowException_WhenGivenEmailInWrongFormat(string email)
        {
            // Arrange
            UserFactory factory = new UserFactory();
            CreateUserResponseDto dto = Impression.Of<CreateUserResponseDto>()
                .With("Email", email)
                .WithDefaults()
                .Create();

            // Act & Assert
            Assert.Throws<Exception>(() => factory.Create(dto));
        }
    }
}
