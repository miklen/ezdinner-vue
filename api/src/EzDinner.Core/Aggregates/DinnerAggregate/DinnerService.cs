using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EzDinner.Core.Aggregates.DinnerAggregate
{
    public class DinnerService : IDinnerService
    {
        private readonly IDinnerRepository _dinnerRepository;

        public DinnerService(IDinnerRepository dinnerRepository)
        {
            _dinnerRepository = dinnerRepository;
        }

        /// <summary>
        /// Returns a <see cref="Dinner"/> object for each day in the span between from and to.
        /// If planned dinner objects are available in storage they are returned for the planned day. If no
        /// planned dinner is available for a day an empty Dinner object is created and returned.
        /// </summary>
        /// <param name="familyId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public async IAsyncEnumerable<Dinner> GetAsync(Guid familyId, DateTime fromDate, DateTime toDate)
        {
            var previousPlannedDinner = fromDate.Date.AddDays(-1);
            await foreach(var dinner in _dinnerRepository.GetAsync(familyId, fromDate, toDate))
            {
                foreach (var unplannedDinner in CreateUnplannedDinners(familyId, previousPlannedDinner.AddDays(1), dinner.Date.AddDays(-1))) yield return unplannedDinner;
                yield return dinner;
                previousPlannedDinner = dinner.Date;
            }
            
            foreach(var unplannedDinner in CreateUnplannedDinners(familyId, previousPlannedDinner.AddDays(1), toDate)) yield return unplannedDinner;
        }


        private IEnumerable<Dinner> CreateUnplannedDinners(Guid familyId, DateTime fromDate, DateTime toDate)
        {
            foreach (var dinner in EachDay(fromDate, toDate).Select(d => new Dinner(familyId, d))) yield return dinner;
        }

        private IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
            {
                yield return day;
            }
        }
    }
}
