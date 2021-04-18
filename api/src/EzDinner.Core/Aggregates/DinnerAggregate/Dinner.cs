﻿using EzDinner.Core.Aggregates.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EzDinner.Core.Aggregates.DinnerAggregate
{
    public class Dinner : AggregateRoot<Guid>
    {
        private readonly List<MenuItem> _menu;
        private readonly List<Tag> _tags;

        public DateTime Date { get; }
        public Guid FamilyId { get; set; }
        public string Description { get; set; }
        public IEnumerable<MenuItem> Menu { get => _menu; }
        public IEnumerable<Tag> Tags { get => _tags; }

        public Dinner(Guid familyId, string description, DateTime date) : base(Guid.NewGuid())
        {
            Date = date;
            FamilyId = familyId;
            Description = description;
            _menu = new List<MenuItem>();
            _tags = new List<Tag>();
        }

        public Dinner(Guid familyId, DateTime date) : this(familyId, "", date)
        {
        }

        /// <summary>
        /// Appends an item to the menu. A menu item does not need to have a receipe specified.
        /// It's possible to have different receipes of the same dish on the menu. Adding a dish/recipe
        /// combination is idempotent.
        /// </summary>
        /// <param name="dishId"></param>
        /// <param name="recipeId"></param>
        public void AddMenuItem(Guid dishId, Guid? recipeId)
        {
            var dishIsAlreadyAdded = _menu.Any(w => w.DishId == dishId && w.ReciepeId == recipeId);
            if (dishIsAlreadyAdded) return;
            _menu.Add(new MenuItem(dishId, recipeId, _menu.Count));
        }

        /// <summary>
        /// Removes a dish/recipe combination from the menu. Does nothing if dish/recipe combination does not
        /// exist on the menu.
        /// </summary>
        /// <param name="dishId"></param>
        /// <param name="recipeId"></param>
        public void RemoveMenuItem(Guid dishId, Guid? receipeId = null)
        {
            var itemOnMenu = _menu.FirstOrDefault(w => w.DishId == dishId && w.ReciepeId == receipeId);
            if (itemOnMenu is null) return;
            _menu.Remove(itemOnMenu);
        }
    }
}
