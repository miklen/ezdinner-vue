using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Core.Aggregates.FamilyMemberAggregate
{
    public class FamilyMember
    {
        public Guid Id { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string FullName => $"{GivenName} {FamilyName}";
    }
}
