using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace UnitTest.Domain.UnitTest
{
    public class ResourceEntityTest
    {
        [Fact]
        public static void RessourceValidation_ShouldPass_WhenGivenValidInfo()
        {
            // Arrange
            string resourceName = "Luksushytte nr. 5";
            string resourceType = "Hytte";
            int location = 4;
            decimal resourceBasePrice = 899.95M;

            // Act
            Resource ressource = new Resource(resourceName, resourceType, resourceBasePrice, location, null);

            // Assert
            Assert.Equal(resourceName, ressource.Name);
            Assert.Equal(resourceType, ressource.Type);
            Assert.Equal(resourceBasePrice, ressource.BasePrice);
        }
        [Fact]
        public static void RessourceValidation_ShouldFail_IfNameIsNull()
        {
            // Arrange
            string resourceName = null;
            string resourceType = "Hytte";
            int location = 4;
            decimal resourceBasePrice = 899.95M;

            // Assert
            Assert.Throws<Exception>(() => new Resource(resourceName, resourceType, resourceBasePrice, location, null));
        }
        [Fact]
        public static void RessourceValidation_ShouldFail_IfTypeIsNull()
        {
            // Arrange
            string resourceName = "Luksushytte nr. 5";
            string resourceType = null;
            int location = 4;
            decimal resourceBasePrice = 899.95M;

            // Assert
            Assert.Throws<Exception>(() => new Resource(resourceName, resourceType, resourceBasePrice, location, null));
        }

    }
}
