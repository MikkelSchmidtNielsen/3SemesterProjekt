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
            int resourceId = 1;
            string resourceName = "Luksushytte nr. 5";
            string resourceType = "Hytte";
            decimal resourceBasePrice = 899.95M;

            // Act
            Resource ressource = new Resource(resourceId, resourceName, resourceType, resourceBasePrice, null);

            // Assert
            Assert.Equal(resourceId, ressource.Id);
            Assert.Equal(resourceName, ressource.Name);
            Assert.Equal(resourceType, ressource.Type);
            Assert.Equal(resourceBasePrice, ressource.BasePrice);
        }
        [Fact]
        public static void RessourceValidation_ShouldFail_IfNameIsNull()
        {
            // Arrange
            int resourceId = 1;
            string resourceName = null;
            string resourceType = "Hytte";
            decimal resourceBasePrice = 899.95M;

            // Assert
            Assert.Throws<Exception>(() => new Resource(resourceId, resourceName, resourceType, resourceBasePrice, null));
        }
        [Fact]
        public static void RessourceValidation_ShouldFail_IfTypeIsNull()
        {
            // Arrange
            int resourceId = 1;
            string resourceName = "Luksushytte nr. 5";
            string resourceType = null;
            decimal resourceBasePrice = 899.95M;

            // Assert
            Assert.Throws<Exception>(() => new Resource(resourceId, resourceName, resourceType, resourceBasePrice, null));
        }

    }
}
