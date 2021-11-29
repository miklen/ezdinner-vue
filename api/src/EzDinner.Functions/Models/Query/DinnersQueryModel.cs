using AutoMapper;
using EzDinner.Core.Aggregates.DinnerAggregate;
using EzDinner.Functions.Models.Json;
using Newtonsoft.Json;
using NodaTime;
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
        }

        [JsonProperty]
        [JsonConverter(typeof(LocalDateConverter))]
        public LocalDate Date { get; set; }
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
