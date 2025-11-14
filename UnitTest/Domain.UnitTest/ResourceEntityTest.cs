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
            double resourceBasePrice = 899.95;

            // Act
            Resource ressource = new Resource(resourceId, resourceName, resourceType, resourceBasePrice);

            // Assert
            Assert.Equal(resourceId, ressource.ResourceID);
            Assert.Equal(resourceName, ressource.ResourceName);
            Assert.Equal(resourceType, ressource.ResourceType);
            Assert.Equal(resourceBasePrice, ressource.ResourceBasePrice);
        }

        [Fact]
        public static void RessourceValidation_ShouldFail_IfIdIsZero()
        {
            // Arrange
            int resourceId = 0;
            string resourceName = "Luksushytte nr. 5";
            string resourceType = "Hytte";
            double resourceBasePrice = 899.95;

            // Assert
            Assert.Throws<Exception>(() => new Resource(resourceId, resourceName, resourceType, resourceBasePrice));
        }
        [Fact]
        public static void RessourceValidation_ShouldFail_IfNameIsNull()
        {
            // Arrange
            int resourceId = 1;
            string resourceName = null;
            string resourceType = "Hytte";
            double resourceBasePrice = 899.95;

            // Assert
            Assert.Throws<Exception>(() => new Resource(resourceId, resourceName, resourceType, resourceBasePrice));
        }
        [Fact]
        public static void RessourceValidation_ShouldFail_IfTypeIsNull()
        {
            // Arrange
            int resourceId = 1;
            string resourceName = "Luksushytte nr. 5";
            string resourceType = null;
            double resourceBasePrice = 899.95;

            // Assert
            Assert.Throws<Exception>(() => new Resource(resourceId, resourceName, resourceType, resourceBasePrice));
        }

    }
}
