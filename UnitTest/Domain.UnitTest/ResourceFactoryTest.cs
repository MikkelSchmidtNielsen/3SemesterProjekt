using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Domain.UnitTest
{
    public class ResourceFactoryTest
    {
        [Fact]
        public static void ResourceCreation_ShouldPass_WhenGivenValidInfo()
        {
            // Arrange
            string resourceName = "Luksushytte nr. 5";
            string resourceType = "Hytte";
            double resourceBasePrice = 899.95;
            string description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc ac leo et est condimentum bibendum vel sed dolor. Nullam dolor nibh, fermentum at fringilla non, facilisis eget arcu.";
            
            // Act
            // Assert
        }
    }
}
