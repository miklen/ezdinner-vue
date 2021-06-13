using EzDinner.Core.Aggregates.DishAggregate;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Query.Core.DishQueries
{
    public class DishStats
    {
        public Guid DishId { get; set; }
        public LocalDate LastUsed { get; set; }
        public int TimesUsed { get; set; }
    }
}
