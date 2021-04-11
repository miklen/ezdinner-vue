using EzDinner.Core.Aggregates.DishAggregate;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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
        
        public Task SaveAsync(Dish dish)
        {
            return _container.UpsertItemAsync(dish);
        }
    }
}
