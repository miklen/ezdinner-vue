using AutoFixture;
using EzDinner.Core.Aggregates.DinnerAggregate;
using MicroElements.AutoFixture.NodaTime;
using Newtonsoft.Json;
using NodaTime;
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
      var menuItem = new MenuItem(Guid.NewGuid());
      var menuItem2 = new MenuItem(Guid.Parse(menuItem.DishId.ToString()));
      var dinnerCount = dinner.Menu.Count();

      // Act
      dinner.AddMenuItem(menuItem);
      dinner.AddMenuItem(menuItem);

      // Assert
      Assert.Equal(dinnerCount + 1, dinner.Menu.Count());
    }

    [Fact]
    public void Menu_ItemCombination_CanBeRemoved()
    {
      // Arrange
      var dinner = Fixture.Build<Dinner>().Create();
      var menuItem = new MenuItem(Guid.NewGuid());
      var dinnerCount = dinner.Menu.Count();

      // Act
      dinner.AddMenuItem(menuItem);
      dinner.RemoveMenuItem(menuItem);

      // Assert
      Assert.Equal(dinnerCount, dinner.Menu.Count());
    }

    [Fact]
    public void Menu_ItemCombination_IsOnlyRemoved()
    {
      // Arrange
      var dinner = Fixture.Build<Dinner>().Create();
      var menuItem = new MenuItem(Guid.NewGuid());
      var menuItem2 = new MenuItem(Guid.NewGuid());
      var dinnerCount = dinner.Menu.Count();

      // Act
      dinner.AddMenuItem(menuItem);
      dinner.AddMenuItem(menuItem2);
      dinner.RemoveMenuItem(menuItem2);

      // Assert
      Assert.Equal(1 + dinnerCount, dinner.Menu.Count());
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
      var menuItem = new MenuItem(Guid.NewGuid());

      // Act
      dinner.AddMenuItem(menuItem);

      // Assert
      Assert.True(dinner.IsPlanned);
    }

    [Fact]
    public void Dinner_HasNoMenuItems_IsPlannedFalse()
    {
      // Arrange
      var dinner = Dinner.CreateNew(Guid.NewGuid(), new LocalDate());

      // Act

      // Assert
      Assert.False(dinner.IsPlanned);
    }

    [Fact]
    public void Menu_Replace_MenuItem()
    {
      // Arrange
      var dinner = Fixture.Build<Dinner>().Create();
      var menuItem = new MenuItem(Guid.NewGuid());
      var menuItem2 = new MenuItem(Guid.NewGuid());
      var dinnerCount = dinner.Menu.Count();

      // Act
      dinner.AddMenuItem(menuItem);

      dinner.ReplaceMenuItem(menuItem, menuItem2);

      // Assert
      Assert.Contains(menuItem2, dinner.Menu);
      Assert.DoesNotContain(menuItem, dinner.Menu);
    }

    [Fact]
    public void Menu_Replace_MenuItem_ByValue_NotReference()
    {
      // Arrange
      var dinner = Fixture.Build<Dinner>().Create();
      var menuItem = new MenuItem(Guid.NewGuid());
      var menuItem2 = new MenuItem(Guid.NewGuid());

      // Act
      dinner.AddMenuItem(menuItem);

      dinner.ReplaceMenuItem(new MenuItem(menuItem.DishId), menuItem2);

      // Assert
      Assert.Contains(menuItem2, dinner.Menu);
      Assert.DoesNotContain(menuItem, dinner.Menu);
    }

    [Fact]
    public void Menu_Replace_Preserves_Ordering()
    {
      // Arrange
      var dinner = Fixture.Build<Dinner>().Create();
      var menuItem = new MenuItem(Guid.NewGuid());
      var menuItem2 = new MenuItem(Guid.NewGuid());
      var menuItem3 = new MenuItem(Guid.NewGuid());
      var menu = dinner.Menu.ToList();

      dinner.ReplaceMenuItem(menu[0], menuItem);
      dinner.ReplaceMenuItem(menu[1], menuItem2);
      dinner.ReplaceMenuItem(menu[2], menuItem3);

      menu = dinner.Menu.ToList();

      Assert.Equal(menuItem, menu[0]);
      Assert.Equal(menuItem2, menu[1]);
      Assert.Equal(menuItem3, menu[2]);
    }
  }
}
