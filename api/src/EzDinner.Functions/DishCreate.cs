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
using Microsoft.Identity.Web.Resource;
using Newtonsoft.Json;

namespace EzDinner.Functions
{
    public class DishCreate
    {
        private readonly ILogger<DishCreate> _logger;
        private readonly IDishRepository _dishRepository;

        public DishCreate(ILogger<DishCreate> logger, IDishRepository dishRepository)
        {
            _logger = logger;
            _dishRepository = dishRepository;
        }
        
        [FunctionName(nameof(DishCreate))]
        [RequiredScope("backendapi")]
        public async Task<IActionResult?> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "dishes")] HttpRequest req
            )
        {
            var (authenticationStatus, authenticationResponse) = await req.HttpContext.AuthenticateAzureFunctionAsync();
            if (!authenticationStatus) return authenticationResponse;

            var newDish = await req.GetBodyAs<CreateDishCommandModel>();
            if (string.IsNullOrWhiteSpace(newDish.Name)) return new BadRequestObjectResult("Name cannot be null or empty");
            if (newDish.FamilyId == Guid.Empty) return new BadRequestObjectResult("FamilyId cannot be empty");

            var dish = new Dish(newDish.FamilyId, newDish.Name);
            await _dishRepository.SaveAsync(dish);

            return new OkObjectResult(dish.Id);
        }
    }
}

