using EzDinner.Core.Aggregates.FamilyAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Functions.Models.Migrate
{
    public class V1Family
    {
        public Guid OwnerId { get; set; }
        public string? Name { get; set; }
        public Guid[]? FamilyMemberIds { get; set; }
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public Family? ConvertToV2()
        {
            if (FamilyMemberIds is null) return null;
            var familyMembers = new List<FamilyMember>() { FamilyMember.CreateOwner(OwnerId) };
            foreach(var familyMemberId in FamilyMemberIds)
            {
                familyMembers.Add(FamilyMember.CreateFamilyMember(familyMemberId));
            }

            var family = new Family(Id, Name!, familyMembers, CreatedDate, UpdatedDate);
            return family;
        }
    }
}
