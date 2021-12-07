using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
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
    public class FamiliesGetFull
    {
        private readonly ILogger<FamiliesGet> _logger;
        private readonly IFamilyQueryService _familyService;
        private readonly IMapper _mapper;

        public FamiliesGetFull(ILogger<FamiliesGet> logger, IMapper mapper, IFamilyQueryService familyService)
        {
            _logger = logger;
            _familyService = familyService;
            _mapper = mapper;
        }

        [FunctionName(nameof(FamiliesGetFull))]
        [RequiredScope("backendapi")]
        public async Task<IActionResult?> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "families")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var (authenticationStatus, authenticationResponse) = await req.HttpContext.AuthenticateAzureFunctionAsync();
            if (!authenticationStatus) return authenticationResponse;

            var families = await _familyService.GetFamiliesDetailsAsync(Guid.Parse(req.HttpContext.User.GetNameIdentifierId()!));

            return new OkObjectResult(families);
        }
    }
}

