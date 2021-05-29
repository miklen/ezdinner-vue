using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Core.Aggregates.FamilyAggregate
{
    public class Family : AggregateRoot<Guid>
    {
        private readonly List<Guid> _familyMemberIds;

        public Guid OwnerId { get; }
        public string Name { get; private set; }
        public IEnumerable<Guid> FamilyMemberIds => _familyMemberIds;

        public DateTime CreatedDate { get; }
        public DateTime UpdatedDate { get; private set; }

        /// <summary>
        /// For deserialization purpose only. One argument for each property to be deserialized.
        /// 
        /// This ctor breaks encapsulation and doesn't enforce invariants. 
        /// Since the Cosmos Client uses it's own CosmosJsonSerializer, then we have to be careful with clever serialization tricks.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ownerId"></param>
        /// <param name="name"></param>
        /// <param name="familyMemberIds"></param>
        /// <param name="createdDate"></param>
        /// <param name="updatedDate"></param>
        public Family(Guid id, Guid ownerId, string name, List<Guid> familyMemberIds, DateTime createdDate, DateTime updatedDate) : base(id)
        {
            OwnerId = ownerId;
            Name = name;
            _familyMemberIds = familyMemberIds;
            CreatedDate = createdDate;
            UpdatedDate = updatedDate;
        }

        public static Family CreateNew(Guid ownerId, string name)
        {
            if (ownerId == Guid.Empty) throw new ArgumentException($"'{nameof(ownerId)}' cannot be empty", nameof(ownerId));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));

            var createdDate = DateTime.UtcNow;
            return new Family(id: Guid.NewGuid(), ownerId, name, familyMemberIds: new List<Guid>(), createdDate, updatedDate: createdDate);
        }

        /// <summary>
        /// Update family name
        /// </summary>
        /// <param name="name"></param>
        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            Name = name;
        }

        /// <summary>
        /// For now an invitation directly adds a family member to the family.
        /// TODO: Add add invitation state, so that you cannot force familyMembers to join
        /// </summary>
        /// <param name="familyMemberId"></param>
        public void InviteFamilyMember(Guid familyMemberId)
        {
            if (_familyMemberIds.Contains(familyMemberId)) return;
            _familyMemberIds.Add(familyMemberId);
            UpdatedDate = DateTime.UtcNow;
        }

        /// <summary>
        /// Remove a family member from family
        /// </summary>
        /// <param name="familyMemberId"></param>
        public void RemoveFamilyMember(Guid familyMemberId)
        {
            _familyMemberIds.Remove(familyMemberId);
            UpdatedDate = DateTime.UtcNow;
        }
    }
}
