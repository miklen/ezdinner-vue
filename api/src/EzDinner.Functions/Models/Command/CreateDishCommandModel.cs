using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Functions.Models.Command
{
    public class CreateDishCommandModel
    {
        public string? Name { get; set; }
        public Guid FamilyId { get; set; }
    }
}
