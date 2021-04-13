using EzDinner.Core.Aggregates;
using System;
using System.Text.Json;
using Xunit;

namespace EzDinner.UnitTests
{
    public class AggregateRootTests
    {
        [Fact]
        public void Instance_IsInitialized_CanBeSerialized()
        {
            var root = new AggregateRoot() { Id = Guid.NewGuid() };

            var json = JsonSerializer.Serialize(root);

            // no exception is good test
        }

        [Fact]
        public void Instance_IsInitialized_CanBeDeserialized()
        {
            var root = new AggregateRoot() { Id = Guid.NewGuid() };
            var json = JsonSerializer.Serialize(root);

            var result = JsonSerializer.Deserialize<AggregateRoot>(json);

            Assert.Equal(root.Id, result.Id);
            // Verifies that the lambda property can be serialized and deserialized correctly
            Assert.Equal(root.PartitionKey, result.PartitionKey);
        }
    }
}