using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using EzDinner.Functions.Models.Query;
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
    public class GetFamilies
    {
        private readonly ILogger<GetFamilies> _logger;

        public GetFamilies(ILogger<GetFamilies> logger)
        {
            _logger = logger;
        }
        
        [FunctionName("GetFamilies")]
        [RequiredScope("backendapi")] // The Azure Function will only accept tokens 1) for users, and 2) having the "access_as_user" scope for this API
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var (authenticationStatus, authenticationResponse) = await req.HttpContext.AuthenticateAzureFunctionAsync();
            if (!authenticationStatus) return authenticationResponse;


            var families = new List<FamilyQueryModel>() { new FamilyQueryModel() { Id = Guid.NewGuid(), Name = "Familien Nygaard"}, new FamilyQueryModel() { Id = Guid.NewGuid(), Name = "Familien Bengtson" } };
            return new OkObjectResult(families);
        }
    }
}

