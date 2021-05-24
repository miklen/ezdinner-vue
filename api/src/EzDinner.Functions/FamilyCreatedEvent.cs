using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EzDinner.Core.Aggregates.FamilyAggregate;
using EzDinner.Functions.Authorization;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using NetCasbin;
using Newtonsoft.Json;

namespace EzDinner.Functions
{
    public class FamilyCreatedEvent
    {
        private readonly ILogger<FamilyCreatedEvent> _logger;
        private readonly Enforcer _enforcer;

        public FamilyCreatedEvent(ILogger<FamilyCreatedEvent> logger, Enforcer enforcer)
        {
            _logger = logger;
            _enforcer = enforcer;
        }
        
        [FunctionName(nameof(FamilyCreatedEvent))]
        public async Task Run([CosmosDBTrigger(
            databaseName: "EzDinner",
            collectionName: "Families",
            ConnectionStringSetting = "CosmosDb:ConnectionString",
            LeaseCollectionName = "FamiliesLeases")] IReadOnlyList<Document> input)
        {
            if (input != null && input.Count > 0)
            {
                _logger.LogInformation("Documents modified " + input.Count);
                _logger.LogInformation("First document Id " + input[0].Id);

                foreach(var document in input)
                {
                    var family = JsonConvert.DeserializeObject<Family>(document.ToString());
                    if (!IsCreatedAction(family)) continue;

                    await CreateOwnerRoleForFamily(family);
                    await AssignOwnerRoleToCreator(family);
                }
            }
        }   

        private Task CreateOwnerRoleForFamily(Family family)
        {
            if (!_enforcer.HasGroupingPolicy(Roles.Owner, family.Id.ToString()))
            {
                // Assign Owner role to the user who created the family
                return _enforcer.AddPolicyAsync(Roles.Owner, family.Id.ToString());
            }
            return Task.CompletedTask;
        }
        
        private Task AssignOwnerRoleToCreator(Family family)
        {
            if (!_enforcer.HasGroupingPolicy(family.OwnerId.ToString(), Roles.Owner, family.Id.ToString()))
            {
                // Create Owner role for the created famiy
                return _enforcer.AddGroupingPolicyAsync(family.OwnerId.ToString(), Roles.Owner, family.Id.ToString());
            }
            return Task.CompletedTask;
        }

        private static bool IsCreatedAction(Family family)
        {
            return family.CreatedDate.Equals(family.UpdatedDate);
        }
    }
}
