using System;
using System.Threading.Tasks;
using EzDinner.Application.Commands.Dishes;
using EzDinner.Authorization.Core;
using EzDinner.Core.Aggregates.DishAggregate;
using EzDinner.Functions.Models.Command;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;

namespace EzDinner.Functions
{
    public class DishDelete
    {
        private readonly ILogger<DishDelete> _logger;
        private readonly IDishRepository _dishRepository;
        private readonly IAuthzService _authz;

        public DishDelete(ILogger<DishDelete> logger, IDishRepository dishRepository, IAuthzService authz)
        {
            _logger = logger;
            _dishRepository = dishRepository;
            _authz = authz;
        }

        [FunctionName(nameof(DishDelete))]
        [RequiredScope("backendapi")]
        public async Task<IActionResult?> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "dishes/family/{familyId}/id/{dishId}")] HttpRequest req,
            string familyId,
            string dishId
            )
        {
            var (authenticationStatus, authenticationResponse) = await req.HttpContext.AuthenticateAzureFunctionAsync();
            if (!authenticationStatus) return authenticationResponse;
            if (!_authz.Authorize(req.HttpContext.User.GetNameIdentifierId()!, familyId, Resources.Dish, Actions.Delete)) return new UnauthorizedResult();

            try
            {
                var dishCommand = new DeleteDishCommand(_dishRepository);
                await dishCommand.Handle(Guid.Parse(familyId), Guid.Parse(dishId));
                return new OkResult();
            }
            catch (Exception e)
            {
                if (e is ArgumentException || e is ArgumentNullException) return new BadRequestObjectResult(e.Message);
                throw;
            }
        }
    }
}

