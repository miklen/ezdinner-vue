using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EzDinner.Authorization;
using EzDinner.Core.Aggregates.FamilyAggregate;
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
        private readonly IPermissionService _permissionService;

        public FamilyCreatedEvent(ILogger<FamilyCreatedEvent> logger, IPermissionService permissionService)
        {
            _logger = logger;
            _permissionService = permissionService;
        }

        /// <summary>
        /// Policy (entries in permission database):
        /// p, admin, domain1, data1, read
        /// p, admin, domain1, data1, write
        /// p, admin, domain2, data2, read
        /// p, admin, domain2, data2, write
        /// p, owner, domain2

        /// g, alice, admin, domain1
        /// g, bob, admin, domain2
        /// g, mikkel, owner, domain2
        /// 
        /// Enforcement results:
        /// alice, domain1, data2, read == false
        /// alice, domain2, data3, read == false
        /// mikkel, domain2, data2, write == true
        /// mikkel, domain2, data3, write == true
        /// mikkel, domain1, data1, write == false
        /// bob, domain2, data3, write == false
        /// bob, domain2, data2, write == true
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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
                    await _permissionService.CreateOwnerRole(family.Id);
                    await _permissionService.AssignRoleToUser(family.OwnerId, Roles.Owner, family.Id);
                }
            }
        }   
    }
}
