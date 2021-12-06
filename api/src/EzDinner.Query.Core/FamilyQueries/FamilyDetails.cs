using EzDinner.Core.Aggregates.FamilyAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Query.Core.FamilyQueries
{
    public class FamilyDetails
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public string? Name { get; set; }
        public IEnumerable<FamilyMember>? FamilyMembers { get; set; }
        public DateTime CreatedDate { get; }
        public DateTime UpdatedDate { get; private set; }
    }
}
