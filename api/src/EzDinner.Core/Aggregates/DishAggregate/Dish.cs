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
        
        /// <summary>
        /// Rating in 10 steps. Values are between 0-10.
        /// </summary>
        public int Rating { get; private set; }

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

        public void SetRating(int rating)
        {
            if (rating > 10 || rating < 0) throw new ArgumentException("Rating must be between 0 and 10");
            Rating = rating;
        }
    }
}
 