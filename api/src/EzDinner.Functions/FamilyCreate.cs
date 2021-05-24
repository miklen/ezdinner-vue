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
    public class FamilyCreate
    {
        private readonly ILogger<FamilyCreate> _logger;
        private readonly IFamilyRepository _familyRepository;

        public FamilyCreate(ILogger<FamilyCreate> logger, IFamilyRepository familyRepository)
        {
            _logger = logger;
            _familyRepository = familyRepository;
        }

        [FunctionName(nameof(FamilyCreate))]
        [RequiredScope("backendapi")]
        public async Task<IActionResult?> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "families")] HttpRequest req
            )
        {
            var (authenticationStatus, authenticationResponse) = await req.HttpContext.AuthenticateAzureFunctionAsync();
            if (!authenticationStatus) return authenticationResponse;

            var newFamily = await req.GetBodyAs<CreateFamilyCommandModel>();
            if (string.IsNullOrWhiteSpace(newFamily.Name)) return new BadRequestObjectResult("Name cannot be null or empty");

            await _familyRepository.SaveAsync(Family.CreateNew(Guid.Parse(req.HttpContext.User.GetNameIdentifierId() ?? ""), newFamily.Name));

            return new OkResult();
        }
    }
}

