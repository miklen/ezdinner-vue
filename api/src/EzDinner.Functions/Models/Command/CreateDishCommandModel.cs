using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Functions.Models.Command
{
    public class CreateDishCommandModel
    {
        /// <summary>
        /// The name of the dish
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// The family to which this dish is added
        /// </summary>
        public Guid FamilyId { get; set; }
    }
}
