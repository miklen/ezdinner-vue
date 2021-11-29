using EzDinner.Core.Aggregates.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Core.Aggregates.DishAggregate
{
    public class Dish : AggregateRoot<Guid>
    {
        public string Name { get; set; }
        public Guid FamilyId { get; }
        public bool Deleted { get; private set; }
        public Uri? Url { get; set; }
        public IEnumerable<Tag> Tags { get; }
        public string Notes { get; set; }
        public int Rating { get; set; }

        /// <summary>
        /// For serialization purpose only. Does not protect invariants and constraints.
        /// </summary>
        public Dish(Guid id, Guid familyId, string name, Uri? url, IEnumerable<Tag> tags, string notes, int rating, bool deleted) : base(id)
        {
            FamilyId = familyId;
            Name = name;
            Url = url;
            Tags = tags;
            Notes = notes;
            Rating = rating;
            Deleted = deleted;
        }

        public static Dish CreateNew(Guid familyId, string name)
        {
            return new Dish(id: Guid.NewGuid(), familyId, name, null, new List<Tag>(), "", 0, deleted: false);
        }

        public void Delete()
        {
            Deleted = true;
        }
    }
}
 