using EzDinner.Core.Aggregates.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Core.Aggregates.DinnerAggregate
{
    public class Dinner
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public IEnumerable<MenuItem> Menu { get; set; }
        public IEnumerable<Tag> Tags { get; set; }

        public Dinner(string description, DateTime date, Guid dishId)
        {
            Description = description;
            Menu = new List<MenuItem>();
            Tags = new List<Tag>();
        }
    }
}
