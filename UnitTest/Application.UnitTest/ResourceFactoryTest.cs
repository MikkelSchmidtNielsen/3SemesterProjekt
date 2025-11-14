using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Application.UnitTest
{
    public class ResourceFactoryTest
    {
        [Fact]
        public static void ResourceCreation_ShouldPass_WhenGivenCorrectInfo()
        {
            // Arrange
            int resourceId = 1;
            string resourceName = "Luksushytte nr. 5";
            string resourceType = "Hytte";
            double resourceBasePrice = 899.95;

            // Act

            // Assert
        }
    }
}
