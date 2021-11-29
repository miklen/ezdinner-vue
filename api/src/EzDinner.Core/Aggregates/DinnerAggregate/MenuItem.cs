using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Core.Aggregates.DinnerAggregate
{
    public class MenuItem : IEquatable<MenuItem>
    {
        public Guid DishId { get; }

        public MenuItem(Guid dishId)
        {
            DishId = dishId;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as MenuItem);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DishId);
        }

        public bool Equals(MenuItem? other)
        {
            return other != null && DishId.Equals(other.DishId);
        }
    }
}
