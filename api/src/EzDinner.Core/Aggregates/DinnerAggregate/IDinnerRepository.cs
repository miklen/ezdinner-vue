using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EzDinner.Core.Aggregates.DinnerAggregate
{
    public interface IDinnerRepository
    {
        Task SaveAsync(Dinner dinner);
        Task<Dinner?> GetAsync(Guid familyId, DateTime exactDate);
        IAsyncEnumerable<Dinner> GetAsync(Guid familyId, DateTime fromDate, DateTime toDate);
    }
}
