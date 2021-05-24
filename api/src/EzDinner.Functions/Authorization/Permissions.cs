using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Functions.Authorization
{
    public static class Permissions
    {
        public static readonly string Family = $"{nameof(Permissions)}.{nameof(Family)}";
        public static readonly string Dish = $"{nameof(Permissions)}.{nameof(Dish)}";
        public static readonly string Dinner = $"{nameof(Permissions)}.{nameof(Dinner)}";
    }
}
