using AutoFixture;
using EzDinner.Core.Aggregates.DishAggregate;
using EzDinner.Infrastructure;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EzDinner.IntegrationTests.DishRepositoryTests
{
    public class DishRepositoryTests : IClassFixture<StartupFixture>
    {
        private readonly ServiceProvider _provider;
        private readonly IDishRepository _dishRepository;
        private readonly Container _container;

        public DishRepositoryTests(StartupFixture startup)
        {
            _provider = startup.Provider;
            _dishRepository = (IDishRepository)_provider.GetService(typeof(IDishRepository));
            var client = (CosmosClient)_provider.GetService(typeof(CosmosClient));
            var config = (IConfiguration)_provider.GetService(typeof(IConfiguration));
            var dbName = config.GetValue<string>("CosmosDb:Database");
            
            // ignore the risk of using .Result as we're in a test harness and just ensuring that the container exists befopre each test
            var dbResponse = client.CreateDatabaseIfNotExistsAsync(dbName).Result;
            var db = dbResponse.Database;
            var result = db.CreateContainerIfNotExistsAsync(new ContainerProperties(DishRepository.CONTAINER, $"/partitionKey")).Result;
            _container = client.GetContainer(dbName, DishRepository.CONTAINER);
        }

        [Fact]
        public async Task GetDishes_MultipleItems_GetsAll()
        {
            // Arrange
            var fixture = new Fixture();
            var familyId = Guid.NewGuid();
            fixture.FreezeByName(nameof(Dish.FamilyId), familyId);
            fixture.FreezeByName(nameof(Dish.Deleted), false);
            var dishes = new List<Dish>();
            fixture.AddManyTo(dishes);
            try
            {
                foreach (var dish in dishes) await _container.UpsertItemAsync(dish);

                // Act
                var result = await _dishRepository.GetDishesAsync(familyId);

                // Assert
                Assert.Equal(dishes.Count, result.Count());
            } finally
            {
                await _container.DeleteContainerAsync();
            }
        }

        [Fact]
        public async Task GetDishes_MultipleItems_RespectsFamilyId()
        {
            // Arrange
            var fixture = new Fixture();
            var familyId = Guid.NewGuid();
            fixture.Inject(familyId);
            fixture.Inject(false);
            var dishes = new List<Dish>();
            fixture.AddManyTo(dishes);
            try
            {
                foreach (var dish in dishes) await _container.UpsertItemAsync(dish);

                // Act
                var result = await _dishRepository.GetDishesAsync(Guid.NewGuid());

                // Assert
                Assert.Empty(result);
            }
            finally
            {
                await _container.DeleteContainerAsync();
            }
        }

        [Fact]
        public async Task GetDishes_ItemDeleted_ExcludesDeletedItems()
        {
            // Arrange
            var fixture = new Fixture();
            var familyId = Guid.NewGuid();
            fixture.FreezeByName(nameof(Dish.FamilyId), familyId);
            fixture.FreezeByName(nameof(Dish.Deleted), false);
            var dishes = new List<Dish>();
            fixture.AddManyTo(dishes);
            dishes.Last().Delete();
            try
            {
                foreach (var dish in dishes) await _container.UpsertItemAsync(dish);

                // Act
                var result = await _dishRepository.GetDishesAsync(familyId);

                // Assert
                Assert.Equal(dishes.Count-1, result.Count());
            }
            finally
            {
                await _container.DeleteContainerAsync();
            }
        }

        [Fact]
        public async Task GetDishes_AllItemsDeleted_ExcludesDeletedItems()
        {
            // Arrange
            var fixture = new Fixture();
            var familyId = Guid.NewGuid();
            fixture.FreezeByName(nameof(Dish.FamilyId), familyId);
            fixture.FreezeByName(nameof(Dish.Deleted), true);
            var dishes = new List<Dish>();
            fixture.AddManyTo(dishes);
            try
            {
                foreach (var dish in dishes) await _container.UpsertItemAsync(dish);

                // Act
                var result = await _dishRepository.GetDishesAsync(familyId);

                // Assert
                Assert.Empty(result);
            }
            finally
            {
                await _container.DeleteContainerAsync();
            }
        }

        [Fact]
        public async Task GetDish_ItemExists_GetsDish()
        {
            // Arrange
            var fixture = new Fixture();
            var dish = fixture.Build<Dish>().Create();
            try
            {
                await _container.UpsertItemAsync(dish);

                // Act
                var result = await _dishRepository.GetDishAsync(dish.Id);

                // Assert
                Assert.NotNull(result);
            }
            finally
            {
                await _container.DeleteContainerAsync();
            }
        }

        [Fact]
        public async Task GetDish_ItemExists_DeserializesAllPropertiesCorrectly()
        {
            // Arrange
            var fixture = new Fixture();
            var dish = fixture.Build<Dish>().Create();
            dish.Delete();
            try
            {
                await _container.UpsertItemAsync(dish);

                // Act
                var result = await _dishRepository.GetDishAsync(dish.Id);

                // Assert
                Assert.Equal(JsonConvert.SerializeObject(dish), JsonConvert.SerializeObject(result));
            }
            finally
            {
                await _container.DeleteContainerAsync();
            }
        }
    }
}
