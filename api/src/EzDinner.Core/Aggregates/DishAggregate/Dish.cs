using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Core.Aggregates.DishAggregate
{
    public class Dish : AggregateRoot
    {
        public string Name { get; }
        public Guid FamilyId { get; }
        public IEnumerable<Recipe> Recipes { get; }

        public Dish(Guid familyId, string name)
        {
            Id = Guid.NewGuid();
            Recipes = new List<Recipe>();
            FamilyId = familyId;
            Name = name;
        }
    }
}
