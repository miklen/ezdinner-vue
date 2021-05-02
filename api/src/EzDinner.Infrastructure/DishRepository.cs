using EzDinner.Core.Aggregates.DishAggregate;
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
    public class DishRepository : IDishRepository
    {
        private readonly CosmosClient _client;
        private readonly Container _container;

        public DishRepository(CosmosClient client, IConfiguration configuration)
        {
            _client = client;
            _container = _client.GetContainer(configuration.GetValue<string>("CosmosDb:Database"), "Dishes");
        }

        public async Task<IEnumerable<Dish>> GetDishesAsync(Guid familyId)
        {
            using (var iterator = _container.GetItemLinqQueryable<Dish>()
                .Where(w => w.FamilyId == familyId)
                .ToFeedIterator())
            {
                var dishes = new List<Dish>();
                
                while (iterator.HasMoreResults)
                {
                    foreach (var family in await iterator.ReadNextAsync())
                    {
                        dishes.Add(family);
                    }
                }
                return dishes;
            }
        }

        public Task SaveAsync(Dish dish)
        {
            return _container.UpsertItemAsync(dish);
        }
    }
}
