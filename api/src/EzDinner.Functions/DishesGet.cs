using System;
using System.Linq;
using System.Threading.Tasks;
using EzDinner.Authorization.Core;
using EzDinner.Core.Aggregates.DishAggregate;
using EzDinner.Functions.Models.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;

namespace EzDinner.Functions
{
    public class DishesGet
    {
        private readonly ILogger<DishCreate> _logger;
        private readonly IDishRepository _dishRepository;
        private readonly IAuthzService _authz;

        public DishesGet(ILogger<DishCreate> logger, IDishRepository dishRepository, IAuthzService authz)
        {
            _logger = logger;
            _dishRepository = dishRepository;
            _authz = authz;
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
            if (!_authz.Authorize(req.HttpContext.User.GetNameIdentifierId(), familyId, Resources.Dish, Actions.Read)) return new UnauthorizedResult();

            _logger.LogInformation("GetDishes called for familyId " + familyId);
            var parsedId = Guid.Parse(familyId);
            var dishes = await _dishRepository.GetDishesAsync(parsedId);

            return new OkObjectResult(dishes.SelectMany(DishesQueryModel.FromDomain));
        }
    }
}

