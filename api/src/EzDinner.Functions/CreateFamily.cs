using System;
using System.IO;
using System.Threading.Tasks;
using EzDinner.Core.Aggregates.FamilyAggregate;
using EzDinner.Functions.Models.Command;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using Newtonsoft.Json;

namespace EzDinner.Functions
{
    public class CreateFamily
    {
        private readonly ILogger<CreateFamily> _logger;

        public CreateFamily(ILogger<CreateFamily> logger)
        {
            _logger = logger;
        }

        [FunctionName("family")]
        [RequiredScope("backendapi")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "EzDinner",
                collectionName: "Families",
                ConnectionStringSetting = "CosmosDBConnectionString",
                PartitionKey = "partitionKey",
                CreateIfNotExists = true
                )]
                IAsyncCollector<Family> cosmosDbFamilies
            )
        {
            var (authenticationStatus, authenticationResponse) = await req.HttpContext.AuthenticateAzureFunctionAsync();
            if (!authenticationStatus) return authenticationResponse;

            var newFamily = await req.GetBodyAs<CreateFamilyCommandModel>();
            await cosmosDbFamilies.AddAsync(new Family(Guid.Parse(req.HttpContext.User.GetNameIdentifierId()), newFamily.Name));

            return new OkResult();
        }
    }
}

