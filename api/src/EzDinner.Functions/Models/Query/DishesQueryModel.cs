using EzDinner.Core.Aggregates.DishAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EzDinner.Functions.Models.Query
{
    /// <summary>
    /// A flattened variant of a dish/recipe combination. 
    /// This model is primarly used for selecing menu items for a dinner.
    /// </summary>
    public class DishesQueryModel
    {
        public Guid DishId { get; set; }
        public string? DishName { get; set; }
        public Guid? RecipeId { get; set; }
        public string? RecipeName { get; set; }

        public static IEnumerable<DishesQueryModel> FromDomain(Dish dish)
        {
            if (dish is null) throw new ArgumentNullException(nameof(dish));
            if (!dish.Recipes.Any()) return new[] { new DishesQueryModel { DishId = dish.Id, DishName = dish.Name } };
            return dish.Recipes.Select(recipe => new DishesQueryModel() { DishId = dish.Id, DishName = dish.Name, RecipeId = recipe.Id, RecipeName = recipe.Name });
        }
    }
}
