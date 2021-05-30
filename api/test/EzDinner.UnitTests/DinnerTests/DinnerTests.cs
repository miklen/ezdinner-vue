using AutoFixture;
using EzDinner.Core.Aggregates.DinnerAggregate;
using MicroElements.AutoFixture.NodaTime;
using Newtonsoft.Json;
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
        private IFixture Fixture => new Fixture().Customize(new NodaTimeCustomization());

        [Fact]
        public void Menu_ItemCombinationExists_IsIdempotent()
        {
            // Arrange
            var dinner = Fixture.Build<Dinner>().Create();
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
            var dinner = Fixture.Build<Dinner>().Create();
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
            var dinner = Fixture.Build<Dinner>().Create();
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
            var dinner = Fixture.Build<Dinner>().Create();
            var dishId = Guid.NewGuid();
            var recipeId = Guid.NewGuid();

            // Act
            dinner.AddMenuItem(dishId, recipeId);
            dinner.AddMenuItem(dishId, null);
            dinner.RemoveMenuItem(dishId, null);

            // Assert
            Assert.Single(dinner.Menu);
        }

        //[Fact]
        //public void NewtonsoftJson_Dinner_CanSerialize()
        //{
        //    // Arrange
        //    var dinner = Fixture.Create<Dinner>();
        //    dinner.AddMenuItem(Guid.NewGuid());

        //    // Act
        //    var json = JsonConvert.SerializeObject(dinner);
        //    var deserialized = JsonConvert.DeserializeObject<Dinner>(json);

        //    // Assert
        //    Assert.Equal(dinner.Id, deserialized.Id);
        //    Assert.Equal(dinner.FamilyId, deserialized.FamilyId);
        //    Assert.Equal(dinner.Date, deserialized.Date);
        //    Assert.Equal(dinner.Menu.First().DishId, deserialized.Menu.First().DishId);
        //}

        [Fact]
        public void Dinner_HasMenuItems_IsPlannedTrue()
        {
            // Arrange
            var dinner = Fixture.Create<Dinner>();
            
            // Act
            dinner.AddMenuItem(Guid.NewGuid());

            // Assert
            Assert.True(dinner.IsPlanned);
        }

        [Fact]
        public void Dinner_HasNoMenuItems_IsPlannedFalse()
        {
            // Arrange
            var dinner = Fixture.Create<Dinner>();

            // Act

            // Assert
            Assert.False(dinner.IsPlanned);
        }
    }
}
