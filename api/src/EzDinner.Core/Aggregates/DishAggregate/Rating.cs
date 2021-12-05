using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Core.Aggregates.DishAggregate
{
    public class Rating
    {
        public Guid RaterId { get; }
        public int RatingValue { get; }

        public Rating(Guid raterId, int ratingValue)
        {
            RaterId = raterId;

            if (ratingValue > 10 || ratingValue < 0) throw new ArgumentException("Rating must be between 0 and 10");
            RatingValue = ratingValue;
        }
    }
}
