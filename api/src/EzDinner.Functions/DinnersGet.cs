using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EzDinner.Core.Aggregates.DinnerAggregate;
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
    public class DinnersGet
    {
        private readonly ILogger<DishCreate> _logger;
        private readonly IMapper _mapper;
        private readonly IDinnerService _dinnerService;

        public DinnersGet(ILogger<DishCreate> logger, IMapper mapper, IDinnerService dinnerService)
        {
            _logger = logger;
            _mapper = mapper;
            _dinnerService = dinnerService;
        }
        
        [FunctionName(nameof(DinnersGet))]
        [RequiredScope("backendapi")]
        public async Task<IActionResult?> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "dinners/family/{familyId}/dates/{fromDate}/{toDate}")] HttpRequest req,
            string familyId,
            DateTime fromDate,
            DateTime toDate
            )
        {
            var (authenticationStatus, authenticationResponse) = await req.HttpContext.AuthenticateAzureFunctionAsync();
            if (!authenticationStatus) return authenticationResponse;

            var parsedId = Guid.Parse(familyId);
            var dinners = _dinnerService.GetAsync(parsedId, fromDate, toDate).Select(_mapper.Map<DinnersQueryModel>);
            return new OkObjectResult(dinners);
        }
    }
}

