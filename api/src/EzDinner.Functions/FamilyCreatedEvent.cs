using System.Collections.Generic;
using System.Threading.Tasks;
using EzDinner.Authorization;
using EzDinner.Core.Aggregates.FamilyAggregate;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EzDinner.Functions
{
    public class FamilyCreatedEvent
    {
        private readonly ILogger<FamilyCreatedEvent> _logger;
        private readonly IPermissionService _permissionService;

        public FamilyCreatedEvent(ILogger<FamilyCreatedEvent> logger, IPermissionService permissionService)
        {
            _logger = logger;
            _permissionService = permissionService;
        }
      
        [FunctionName(nameof(FamilyCreatedEvent))]
        public async Task Run([CosmosDBTrigger(
            databaseName: "EzDinner",
            collectionName: "Families",
            ConnectionStringSetting = "CosmosDb:ConnectionString",
            LeaseCollectionName = "FamiliesLeases",
            CreateLeaseCollectionIfNotExists = true)] IReadOnlyList<Document> input)
        {
            if (input != null && input.Count > 0)
            {
                _logger.LogInformation("Documents modified " + input.Count);
                _logger.LogInformation("First document Id " + input[0].Id);

                foreach(var document in input)
                {
                    var family = JsonConvert.DeserializeObject<Family>(document.ToString());
                    await _permissionService.CreateOwnerRole(family.Id);
                    await _permissionService.AssignRoleToUser(family.OwnerId, Roles.Owner, family.Id);
                }
            }
        }   
    }
}
