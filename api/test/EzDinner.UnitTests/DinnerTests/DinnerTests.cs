using AutoFixture;
using EzDinner.Core.Aggregates.DinnerAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EzDinner.UnitTests.DinnerTests
{
    public class DinnerTests
    {
        [Fact]
        public void Menu_ItemCombinationExists_IsIdempotent()
        {
            // Arrange
            var fixture = new Fixture();
            var dinner = fixture.Build<Dinner>().Create();
            var dishId = Guid.NewGuid();
            var recipeId = Guid.NewGuid();

            // Act
            dinner.AddMenuItem(dishId, recipeId);
            dinner.AddMenuItem(Guid.Parse(dishId.ToString()), Guid.Parse(recipeId.ToString()));

            // Assert
            Assert.Single(dinner.Menu);
        }

        [Fact]
        public void Menu_NullRecipe_IsValidCombination()
        {
            // Arrange
            var fixture = new Fixture();
            var dinner = fixture.Build<Dinner>().Create();
            var dishId = Guid.NewGuid();
            var recipeId = Guid.NewGuid();

            // Act
            dinner.AddMenuItem(dishId, recipeId);
            dinner.AddMenuItem(Guid.Parse(dishId.ToString()), null);

            // Assert
            Assert.Equal(2, dinner.Menu.Count());
        }

        [Fact]
        public void Menu_ItemCombination_CanBeRemoved()
        {
            // Arrange
            var fixture = new Fixture();
            var dinner = fixture.Build<Dinner>().Create();
            var dishId = Guid.NewGuid();
            var recipeId = Guid.NewGuid();

            // Act
            dinner.AddMenuItem(dishId, recipeId);
            dinner.RemoveMenuItem(dishId, recipeId);

            // Assert
            Assert.Empty(dinner.Menu);
        }

        [Fact]
        public void Menu_ItemCombination_IsOnlyRemoved()
        {
            // Arrange
            var fixture = new Fixture();
            var dinner = fixture.Build<Dinner>().Create();
            var dishId = Guid.NewGuid();
            var recipeId = Guid.NewGuid();

            // Act
            dinner.AddMenuItem(dishId, recipeId);
            dinner.AddMenuItem(dishId, null);
            dinner.RemoveMenuItem(dishId, null);

            // Assert
            Assert.Single(dinner.Menu);
        }

    }
}
