using Domain.Models;

namespace UnitTest.Domain.UnitTest
{
    public class UserEntityTest
    {
        [Fact]
        public void UserCreation_ShouldPass_WhenGivenCorrectInformation()
        {
            // Arrange
            string mail = "test@system.dk";

            // Act
            User sut = new User(mail);

            // Arrange
            Assert.Equal(mail, sut.Email);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Danline@dk")]
        [InlineData("DanlineDk")]
        [InlineData("@Danline.dk")]
        public void UserCreation_ShouldThrowException_WhenGivenEmailInWrongFormat(string email)
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() => new User(email));
        }
    }
}
