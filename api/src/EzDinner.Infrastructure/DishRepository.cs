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
        public const string CONTAINER = "Dishes";

        public DishRepository(CosmosClient client, IConfiguration configuration)
        {
            _client = client;
            _container = _client.GetContainer(configuration.GetValue<string>("CosmosDb:Database"), CONTAINER);
        }

        public async Task<IEnumerable<Dish>> GetDishesAsync(Guid familyId)
        {
            var sql = $"SELECT * FROM c WHERE c.familyId = '{familyId}'";
            var queryDefinition = new QueryDefinition(sql);
            var queryResultSetIterator = _container.GetItemQueryIterator<Dish>(queryDefinition);
            var dishes = new List<Dish>();

            while (queryResultSetIterator.HasMoreResults)
            {
                foreach (var family in await queryResultSetIterator.ReadNextAsync())
                {
                    dishes.Add(family);
                }
            }
            return dishes;
        }

        public Task SaveAsync(Dish dish)
        {
            return _container.UpsertItemAsync(dish);
        }
    }
}
