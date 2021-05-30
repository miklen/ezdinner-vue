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
    public class FamilyCreatedEventUpdatePolicies
    {
        private readonly ILogger<FamilyCreatedEventUpdatePolicies> _logger;
        private readonly IAuthzService _authz;

        public FamilyCreatedEventUpdatePolicies(ILogger<FamilyCreatedEventUpdatePolicies> logger, IAuthzService authz)
        {
            _logger = logger;
            _authz = authz;
        }
      
        [FunctionName(nameof(FamilyCreatedEventUpdatePolicies))]
        public async Task Run([CosmosDBTrigger(
            databaseName: "EzDinner",
            collectionName: "Families",
            ConnectionStringSetting = "CosmosDb:ConnectionString",
            LeaseCollectionName = "Leases",
            LeaseCollectionPrefix = "policies",
            CreateLeaseCollectionIfNotExists = true)] IReadOnlyList<Document> input)
        {
            if (input != null && input.Count > 0)
            {
                _logger.LogInformation("Families updated " + input.Count);
                var updatePermissionsCommand = new UpdateAuthorizationPoliciesCommand(_authz);

                foreach (var document in input)
                {
                    var family = JsonConvert.DeserializeObject<Family>(document.ToString());
                    _logger.LogInformation("Updating policies for family " + family.Id);
                    await updatePermissionsCommand.Handle(family);
                }
            }
        }   
    }
}
