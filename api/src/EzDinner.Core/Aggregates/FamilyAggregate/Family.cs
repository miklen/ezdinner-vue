using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Core.Aggregates.FamilyAggregate
{
    public class Family : AggregateRoot<Guid>
    {
        public Guid OwnerId { get; }
        public string Name { get; set; }
        public List<Guid> FamilyMemberIds { get; }

        public Family(Guid ownerId, string name) : base(Guid.NewGuid())
        {
            if (ownerId == Guid.Empty) throw new ArgumentException($"'{nameof(ownerId)}' cannot be empty", nameof(ownerId));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));

            OwnerId = ownerId;
            Name = name;
            FamilyMemberIds = new List<Guid>();
        }
    }
}
