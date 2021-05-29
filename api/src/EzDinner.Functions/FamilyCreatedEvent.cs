using System.Collections.Generic;
using System.Threading.Tasks;
using EzDinner.Application.Commands;
using EzDinner.Authorization.Core;
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
        private readonly IAuthzService _authz;

        public FamilyCreatedEvent(ILogger<FamilyCreatedEvent> logger, IAuthzService authz)
        {
            _logger = logger;
            _authz = authz;
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
                var updatePermissionsCommand = new UpdateAuthorizationPoliciesCommand(_authz);

                foreach (var document in input)
                {
                    var family = JsonConvert.DeserializeObject<Family>(document.ToString());
                    // Ensure roles exists
                    await updatePermissionsCommand.Handle(family);
                }
            }
        }   
    }
}
