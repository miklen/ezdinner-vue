using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EzDinner.Core.Aggregates.DinnerAggregate
{
    public interface IDinnerService
    {
        /// <summary>
        /// If planned dinner objects are available in storage they are returned for the planned day. If no
        /// planned dinner is available for a day an empty Dinner object is created and returned.
        /// </summary>
        /// <param name="familyId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns>Returns a <see cref="Dinner"/> object for each day in the span between from and to.</returns>
        IAsyncEnumerable<Dinner> GetAsync(Guid familyId, DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Get a dinner object for a date.
        /// </summary>
        /// <param name="familyId"></param>
        /// <param name="exactDate"></param>
        /// <returns>If a dinner is planned for the given date the object is returned from storage. If no dinner is planned a new empty (unplanned) dinner object is returned.</returns>
        Task<Dinner> GetAsync(Guid familyId, DateTime exactDate);
    }
}
