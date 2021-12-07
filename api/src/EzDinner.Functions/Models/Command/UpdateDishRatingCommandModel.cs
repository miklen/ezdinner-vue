using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Functions.Models.Command
{
    public class UpdateDishRatingCommandModel
    {
        public double Rating { get; set; }
        public string? FamilyMemberId { get; set; }

        public int GetRatingInDomainFormat()
        {
            return Convert.ToInt32(Rating * 2d);
        }
    }
}
