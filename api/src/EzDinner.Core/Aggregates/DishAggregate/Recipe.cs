using EzDinner.Core.Aggregates.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Core.Aggregates.DishAggregate
{
    public class Recipe
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Uri? Url { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public string Notes { get; set; }

        public Recipe(string name, string notes)
        {
            Name = name;
            Notes = notes;
            Tags = new List<Tag>();
        }
    }
}
