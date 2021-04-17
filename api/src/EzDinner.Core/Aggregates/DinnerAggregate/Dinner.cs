using EzDinner.Core.Aggregates.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Core.Aggregates.DinnerAggregate
{
    public class Dinner : AggregateRoot<DateTime>
    {
        public DateTime Date { get; }
        public Guid FamilyId { get; set; }
        public string Description { get; set; }
        public IEnumerable<MenuItem> Menu { get; }
        public IEnumerable<Tag> Tags { get; }

        public Dinner(string description, DateTime date) : base(date)
        {
            Description = description;
            Menu = new List<MenuItem>();
            Tags = new List<Tag>();
        }
    }
}
