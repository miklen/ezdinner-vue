using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Core.Aggregates.DishAggregate
{
    public class Dish : AggregateRoot<Guid>
    {
        public string Name { get; set; }
        public Guid FamilyId { get; }
        public IEnumerable<Recipe> Recipes { get; }
        public bool Deleted { get; private set; }

        /// <summary>
        /// For serialization purpose only. Does not protect invariants and constraints.
        /// </summary>
        public Dish(Guid id, Guid familyId, string name, IEnumerable<Recipe> recipes, bool deleted) : base(id)
        {
            FamilyId = familyId;
            Name = name;
            Recipes = recipes;
            Deleted = deleted;
        }

        public static Dish CreateNew(Guid familyId, string name)
        {
            return new Dish(id: Guid.NewGuid(), familyId, name, recipes: new List<Recipe>(), deleted: false);
        }

        public void Delete()
        {
            Deleted = true;
        }
    }
}
 