using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Functions.Models.Query
{
    public class FamilyQueryModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid OwnerId { get; set; }
        public string? OwnerName { get; set; }
    }
}
