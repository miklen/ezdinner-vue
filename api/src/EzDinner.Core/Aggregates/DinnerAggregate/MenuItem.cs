using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Core.Aggregates.DinnerAggregate
{
    public class MenuItem : IEquatable<MenuItem>
    {
        public Guid DishId { get; }
        public Guid? ReciepeId { get; }

        public MenuItem(Guid dishId, Guid? reciepeId = null)
        {
            DishId = dishId;
            ReciepeId = reciepeId;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as MenuItem);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DishId, ReciepeId);
        }

        public bool Equals(MenuItem? other)
        {
            return other != null && DishId.Equals(other.DishId) &&
                   EqualityComparer<Guid?>.Default.Equals(ReciepeId, other.ReciepeId);
        }
    }
}
