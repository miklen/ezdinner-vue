using System;
using System.IO;
using System.Threading.Tasks;
using EzDinner.Core.Aggregates.DishAggregate;
using EzDinner.Functions.Models.Command;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Newtonsoft.Json;

namespace EzDinner.Functions
{
    public class GetDishes
    {
        private readonly ILogger<CreateDish> _logger;
        private readonly IDishRepository _dishRepository;

        public GetDishes(ILogger<CreateDish> logger, IDishRepository dishRepository)
        {
            _logger = logger;
            _dishRepository = dishRepository;
        }
        
        [FunctionName("dishes")]
        public async Task<IActionResult?> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "dishes/family/{familyId}")] HttpRequest req,
            string familyId
            )
        {
            var (authenticationStatus, authenticationResponse) = await req.HttpContext.AuthenticateAzureFunctionAsync();
            if (!authenticationStatus) return authenticationResponse;

            // TODO: Validate that the user has access to this familyId!
            var parsedId = Guid.Parse(familyId);

            var dishes = await _dishRepository.GetDishesAsync(parsedId);

            return new OkObjectResult(dishes);
        }
    }
}

