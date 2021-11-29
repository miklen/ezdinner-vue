using EzDinner.Functions.Models.Json;
using Newtonsoft.Json;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Functions.Models.Command
{
    public class DinnerReplaceMenuItemCommandModel
    {
        /// <summary>
        /// The Id of the family to plan a menu item for
        /// </summary>
        public Guid FamilyId { get; set; }

        /// <summary>
        /// The dishId to add to the menu.
        /// </summary>
        public Guid DishId { get; set; }

        /// <summary>
        /// Id of the Dish that should replace every occurance of this menu item
        /// </summary>
        public Guid NewDishId { get; set; }
    }
}
