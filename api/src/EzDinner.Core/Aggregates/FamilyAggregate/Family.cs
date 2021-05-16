using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Core.Aggregates.FamilyAggregate
{
    public class Family : AggregateRoot<Guid>
    {
        private readonly List<Guid> _familyMemberIds;

        public Guid OwnerId { get; }
        public string Name { get; set; }
        public IEnumerable<Guid> FamilyMemberIds => _familyMemberIds;

        public Family(Guid ownerId, string name) : base(Guid.NewGuid())
        {
            if (ownerId == Guid.Empty) throw new ArgumentException($"'{nameof(ownerId)}' cannot be empty", nameof(ownerId));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));

            OwnerId = ownerId;
            Name = name;
            _familyMemberIds = new List<Guid>();
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
        }

        /// <summary>
        /// Remove a family member from family
        /// </summary>
        /// <param name="familyMemberId"></param>
        public void RemoveFamilyMember(Guid familyMemberId)
        {
            _familyMemberIds.Remove(familyMemberId);
        }
    }
}
