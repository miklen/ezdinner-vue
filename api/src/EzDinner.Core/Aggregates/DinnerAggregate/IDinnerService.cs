using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Core.Aggregates.DinnerAggregate
{
    public interface IDinnerService
    {
        IAsyncEnumerable<Dinner> GetAsync(Guid familyId, DateTime fromDate, DateTime toDate);
    }
}
