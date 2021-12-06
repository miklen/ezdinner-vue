using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EzDinner.Core.Aggregates.FamilyAggregate
{
    public class Family : AggregateRoot<Guid>
    {
        private readonly List<FamilyMember> _familyMembers;

        public Guid OwnerId => _familyMembers.First(w => w.IsOwner).Id;
        public string Name { get; private set; }
        public IEnumerable<FamilyMember> FamilyMembers => _familyMembers;

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
        /// <param name="familyMembers"></param>
        /// <param name="createdDate"></param>
        /// <param name="updatedDate"></param>
        public Family(Guid id, string name, List<FamilyMember> familyMembers, DateTime createdDate, DateTime updatedDate) : base(id)
        {
            Name = name;
            _familyMembers = familyMembers;
            CreatedDate = createdDate;
            UpdatedDate = updatedDate;
        }

        public static Family CreateNew(Guid ownerId, string name)
        {
            if (ownerId == Guid.Empty) throw new ArgumentException($"'{nameof(ownerId)}' cannot be empty", nameof(ownerId));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));

            var createdDate = DateTime.UtcNow;
            return new Family(id: Guid.NewGuid(), name, familyMembers: new List<FamilyMember>() { FamilyMember.CreateOwner(ownerId) }, createdDate, updatedDate: createdDate);
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
            if (_familyMembers.Any(w => w.Id == familyMemberId)) return;
            _familyMembers.Add(FamilyMember.CreateFamilyMember(familyMemberId));
            UpdatedDate = DateTime.UtcNow;
        }

        /// <summary>
        /// Remove a family member from family
        /// </summary>
        /// <param name="familyMemberId"></param>
        public void RemoveFamilyMember(Guid familyMemberId)
        {
            _familyMembers.RemoveAll(w => w.Id == familyMemberId);
            UpdatedDate = DateTime.UtcNow;
        }
    }
}
