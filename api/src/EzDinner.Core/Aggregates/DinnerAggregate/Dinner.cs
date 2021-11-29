using EzDinner.Core.Aggregates.Shared;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EzDinner.Core.Aggregates.DinnerAggregate
{
  public class Dinner : AggregateRoot<Guid>
  {
    private readonly List<MenuItem> _menu;
    private readonly List<Tag> _tags;

    public LocalDate Date { get; }
    public Guid FamilyId { get; private set; }
    public IEnumerable<MenuItem> Menu { get => _menu; }
    public IEnumerable<Tag> Tags { get => _tags; }
    public bool IsPlanned => Menu.Any();


    /// <summary>
    /// For serialization purpose only
    /// </summary>
    public Dinner(Guid id, Guid familyId, LocalDate date, IEnumerable<MenuItem> menu, IEnumerable<Tag> tags) : base(id)
    {
      Date = date;
      FamilyId = familyId;
      _menu = menu.ToList();
      _tags = tags.ToList();
    }

    /// <summary>
    /// Create a new Dinner for a given calendar date. The date is relative to the family and does not consider timezones or time in any way.
    /// </summary>
    /// <param name="familyId"></param>
    /// <param name="date"></param>
    /// <returns></returns>
    public static Dinner CreateNew(Guid familyId, LocalDate date)
    {
      return new Dinner(id: Guid.NewGuid(), familyId, date, menu: new List<MenuItem>(), tags: new List<Tag>());
    }


    /// <summary>
    /// Appends an item to the menu.
    /// </summary>
    /// <param name="dishId"></param>
    public void AddMenuItem(MenuItem menuItem)
    {
      var dishIsAlreadyAdded = _menu.Any(w => w == menuItem);
      if (dishIsAlreadyAdded) return;
      _menu.Add(menuItem);
    }

    /// <summary>
    /// Removes a dish from the menu. Does nothing if dish does not
    /// exist on the menu.
    /// </summary>
    /// <param name="dishId"></param>
    public void RemoveMenuItem(MenuItem menuItem)
    {
      var itemOnMenu = _menu.FirstOrDefault(w => w.Equals(menuItem));
      if (itemOnMenu is null) return;
      _menu.Remove(itemOnMenu);
    }

    public bool ReplaceMenuItem(MenuItem old, MenuItem replacement)
    {
      var menuItemIndex = _menu.FindIndex(w => w.Equals(old));
      if (menuItemIndex == -1) return false;
      _menu[menuItemIndex] = replacement;
      return true;
    }
  }
}
