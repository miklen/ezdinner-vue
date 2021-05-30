using EzDinner.Functions.Models.Json;
using Newtonsoft.Json;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Functions.Models.Command
{
    public class DinnerAddRemoveMenuItemCommandModel
    {
        /// <summary>
        /// The Id of the family to plan a menu item for
        /// </summary>
        public Guid FamilyId { get; set; }

        /// <summary>
        /// Date is the unique identifier for a dinner. You can only have _one_ dinner pr. day
        /// </summary>
        [JsonProperty]
        [JsonConverter(typeof(LocalDateConverter))]
        public LocalDate Date { get; set; }
        /// <summary>
        /// The dishId to add to the menu.
        /// </summary>
        public Guid DishId { get; set; }
        /// <summary>
        /// If a recipe has been selected for the dish, then that is specified as well.
        /// This value may be null as some dishes may not have recipes.
        /// </summary>
        public Guid? RecipeId { get; set; }
    }
}
