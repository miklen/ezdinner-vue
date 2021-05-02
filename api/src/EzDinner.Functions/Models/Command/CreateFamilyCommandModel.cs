using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Functions.Models.Command
{
    public class CreateFamilyCommandModel
    {
        /// <summary>
        /// The name of the family. This is used as a human friendly name to identify
        /// the selection of a family in the frontend.
        /// </summary>
        public string? Name { get; set; }
    }
}
