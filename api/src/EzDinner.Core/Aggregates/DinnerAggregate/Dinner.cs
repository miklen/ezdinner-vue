using EzDinner.Core.Aggregates.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Core.Aggregates.DinnerAggregate
{
    public class Dinner : AggregateRoot<Guid>
    {
        public DateTime Date { get; }
        public Guid FamilyId { get; set; }
        public string Description { get; set; }
        public IEnumerable<MenuItem> Menu { get; }
        public IEnumerable<Tag> Tags { get; }

        public Dinner(Guid familyId, string description, DateTime date) : base(Guid.NewGuid())
        {
            Date = date;
            FamilyId = familyId;
            Description = description;
            Menu = new List<MenuItem>();
            Tags = new List<Tag>();
        }

        public Dinner(Guid familyId, DateTime date) : this(familyId, "", date)
        {
        }
    }
}
