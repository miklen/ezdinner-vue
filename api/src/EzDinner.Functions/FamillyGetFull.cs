using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using EzDinner.Authorization.Core;
using EzDinner.Core.Aggregates.FamilyAggregate;
using EzDinner.Core.Aggregates.UserAggregate;
using EzDinner.Functions.Models.Query;
using EzDinner.Query.Core.FamilyQueries;
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
    public class FamilyGetFull
    {
        private readonly ILogger<FamilyGetFull> _logger;
        private readonly IFamilyQueryService _familyService;
        private readonly IMapper _mapper;
        private readonly IAuthzRepository _authz;

        public FamilyGetFull(ILogger<FamilyGetFull> logger, IMapper mapper, IAuthzRepository authz, IFamilyQueryService familyService)
        {
            _logger = logger;
            _familyService = familyService;
            _mapper = mapper;
            _authz = authz;
        }

        [FunctionName(nameof(FamilyGetFull))]
        [RequiredScope("backendapi")]
        public async Task<IActionResult?> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "families/{familyId}")] HttpRequest req, string familyId)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var (authenticationStatus, authenticationResponse) = await req.HttpContext.AuthenticateAzureFunctionAsync();
            if (!authenticationStatus) return authenticationResponse;
            var userId = Guid.Parse(req.HttpContext.User.GetNameIdentifierId() ?? "");
            if (!_authz.VerifyUserPermission(userId.ToString(), familyId, Resources.Family, Actions.Read)) return new UnauthorizedResult();
            var family = await _familyService.GetFamilyDetailsAsync(Guid.Parse(familyId));
            return new OkObjectResult(family);
        }
    }
}

