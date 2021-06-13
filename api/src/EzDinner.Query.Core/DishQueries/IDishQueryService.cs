using NodaTime;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EzDinner.Query.Core.DishQueries
{
    public interface IDishQueryService
    {
        public Task<Dictionary<Guid, DishStats>> GetDishUsageStatsAsync(Guid familyId, LocalDate from, LocalDate to);
    }
}
