using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Core.Aggregates.UserAggregate
{
    public class User
    {
        public Guid Id { get; set; }
        public string? GivenName { get; set; }
        public string? FamilyName { get; set; }
        public string FullName => $"{GivenName} {FamilyName}";
    }
}
