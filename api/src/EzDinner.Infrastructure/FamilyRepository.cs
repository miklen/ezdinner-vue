using EzDinner.Core.Aggregates.FamilyAggregate;
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
  public class FamilyRepository : IFamilyRepository
  {
    private readonly CosmosClient _client;
    private readonly Container _container;

    public FamilyRepository(CosmosClient client, IConfiguration configuration)
    {
      _client = client;
      _container = _client.GetContainer(configuration.GetValue<string>("CosmosDb:Database"), "Families");
    }

    public Task SaveAsync(Family family)
    {
      return _container.UpsertItemAsync(family);
    }

    public async Task<IEnumerable<Family>> getFamilySelectorsAsync(Guid userId)
    {
      var families = new List<Family>();
      using (var setIterator = _container.GetItemLinqQueryable<Family>()
                 .Where(b => b.OwnerId == userId || b.FamilyMemberIds.Contains(userId))
                 .ToFeedIterator())
      {
        while (setIterator.HasMoreResults)
        {
          foreach (var family in await setIterator.ReadNextAsync())
          {
            families.Add(family);
          }
        }
      }
      return families;
    }

    public async Task<Family?> GetFamily(Guid familyId)
    {
      using (var setIterator = _container.GetItemLinqQueryable<Family>()
                  .Where(b => b.Id == familyId)
                 .ToFeedIterator())
      {
        while (setIterator.HasMoreResults)
        {
          foreach (var family in await setIterator.ReadNextAsync())
          {
            return family;
          }
        }
        return null;
      }
    }
  }
}
