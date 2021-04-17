using EzDinner.Core.Aggregates.DinnerAggregate;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzDinner.Infrastructure
{
    public class DinnerRepository : IDinnerRepository
    {
        private readonly CosmosClient _client;
        private readonly Container _container;

        public DinnerRepository(CosmosClient client, IConfiguration configuration)
        {
            _client = client;
            _container = _client.GetContainer(configuration.GetValue<string>("CosmosDb:Database"), "Dinners");
        }

        public Task<Dinner> GetAsync(Guid familyId, DateTime exactDate)
        {
            throw new NotImplementedException();
        }

        // TODO: Fill with dates not present in database
        public async Task<IEnumerable<Dinner>> GetAsync(Guid familyId, DateTime fromDate, DateTime toDate)
        {
            using (var iterator = _container.GetItemLinqQueryable<Dinner>()
                .Where(w => w.FamilyId == familyId && w.Date > fromDate && w.Date < toDate)
                .ToFeedIterator())
            {
                var dinners = new List<Dinner>();

                while (iterator.HasMoreResults)
                {
                    foreach (var family in await iterator.ReadNextAsync())
                    {
                        dinners.Add(family);
                    }
                }
                return dinners;
            }
        }

        public Task SaveAsync(Dinner dinner)
        {
            return _container.UpsertItemAsync(dinner);
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
