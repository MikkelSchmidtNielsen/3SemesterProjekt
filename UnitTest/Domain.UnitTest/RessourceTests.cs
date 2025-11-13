using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace UnitTest.Domain.UnitTest
{
    public class RessourceTests
    {
        [Fact]
        public static void RessourceValidation_ShouldPass_WhenGivenCorrectInfo()
        {
            // Arrange
            int ressourceId = 1;
            string ressourceName = "Luksushytte nr. 5";
            string ressourceType = "Hytte";
            double ressourceBasePrice = 899.95;

            // Act
            Ressource ressource = new Ressource(ressourceId, ressourceName, ressourceType, ressourceBasePrice);

            // Assert
            Assert.Equal(ressourceId, ressource.ResourceID);
            Assert.Equal(ressourceName, ressource.ResourceName);
            Assert.Equal(ressourceType, ressource.ResourceType);
            Assert.Equal(ressourceBasePrice, ressource.ResourceBasePrice);
        }

        [Fact]
        public static void RessourceValidation_ShouldFail_IfIdIsZero()
        {
            // Arrange
            int ressourceId = 0;
            string ressourceName = "Luksushytte nr. 5";
            string ressourceType = "Hytte";
            double ressourceBasePrice = 899.95;

            // Assert
            Assert.Throws<Exception>(() => new Ressource(ressourceId, ressourceName, ressourceType, ressourceBasePrice));
        }
        [Fact]
        public static void RessourceValidation_ShouldFail_IfNameIsNull()
        {
            // Arrange
            int ressourceId = 1;
            string ressourceName = null;
            string ressourceType = "Hytte";
            double ressourceBasePrice = 899.95;

            // Assert
            Assert.Throws<Exception>(() => new Ressource(ressourceId, ressourceName, ressourceType, ressourceBasePrice));
        }
        [Fact]
        public static void RessourceValidation_ShouldFail_IfTypeIsNull()
        {
            // Arrange
            int ressourceId = 1;
            string ressourceName = "Luksushytte nr. 5";
            string ressourceType = null;
            double ressourceBasePrice = 899.95;

            // Assert
            Assert.Throws<Exception>(() => new Ressource(ressourceId, ressourceName, ressourceType, ressourceBasePrice));
        }

    }
}
