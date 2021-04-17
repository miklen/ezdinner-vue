using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Core.Aggregates.DinnerAggregate
{
    public class MenuItem
    {
        public Guid DishId { get; }
        public Guid? ReciepeId { get; }
        public int Ordering { get; set; }
    }
}
