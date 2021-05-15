using AutoMapper;
using EzDinner.Core.Aggregates.DinnerAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Functions.Models.Query
{
    public class DinnersQueryModel
    {
        public class MenuItemQueryModel
        {
            public Guid DishId { get; set; }
            public Guid? RecipeId { get; set; }
        }
        
        public DateTime Date { get; set; }
        public IEnumerable<MenuItemQueryModel>? Menu { get; set; }
        public bool IsPlanned { get; set; }
    }

    public class DinnersMapping : Profile
    {
        public DinnersMapping()
        {
            CreateMap<Dinner, DinnersQueryModel>();
            CreateMap<MenuItem, DinnersQueryModel.MenuItemQueryModel>();
        }
    }
}
