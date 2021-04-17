using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EzDinner.Core.Aggregates.DishAggregate;
using EzDinner.Functions.Models.Command;
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
    public class DishesGet
    {
        private readonly ILogger<DishCreate> _logger;
        private readonly IDishRepository _dishRepository;

        public DishesGet(ILogger<DishCreate> logger, IDishRepository dishRepository)
        {
            _logger = logger;
            _dishRepository = dishRepository;
        }
        
        [FunctionName(nameof(DishesGet))]
        [RequiredScope("backendapi")]
        public async Task<IActionResult?> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "dishes/family/{familyId}")] HttpRequest req,
            string familyId
            )
        {
            var (authenticationStatus, authenticationResponse) = await req.HttpContext.AuthenticateAzureFunctionAsync();
            if (!authenticationStatus) return authenticationResponse;

            // TODO: Validate that the user has access to this familyId!
            _logger.LogInformation("GetDishes called for familyId " + familyId);
            var parsedId = Guid.Parse(familyId);
            var dishes = await _dishRepository.GetDishesAsync(parsedId);

            return new OkObjectResult(dishes.SelectMany(DishesQueryModel.FromDomain));
        }
    }
}

