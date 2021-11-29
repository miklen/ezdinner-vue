using EzDinner.Core.Aggregates.DishAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EzDinner.Functions.Models.Query
{
  /// <summary>
  /// Dish query model
  /// </summary>
  public class DishesQueryModel
  {
    public Guid Id { get; set; }
    public string? Name { get; set; }

    // TODO: Switch to AutoMapper
    public static DishesQueryModel FromDomain(Dish dish)
    {
      if (dish is null) throw new ArgumentNullException(nameof(dish));
      return new DishesQueryModel { Id = dish.Id, Name = dish.Name };
    }
  }
}
