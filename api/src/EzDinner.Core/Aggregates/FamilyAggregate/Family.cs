using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Core.Aggregates.FamilyAggregate
{
    public class Family
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public string Name { get; set; }
        public List<Guid> FamilyMembers { get; set; }
    }
}
