using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Core.Aggregates.DinnerAggregate
{
    public class MenuItem
    {
        public Guid DishId { get; set; }
        public Guid? ReciepeId { get; set; }
        public int Ordering { get; set; }
    }
}
