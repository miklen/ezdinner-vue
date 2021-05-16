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
    public class FamiliesGet
    {
        private readonly ILogger<FamiliesGet> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IFamilyRepository _familyRepository;
        private readonly IMapper _mapper;

        public FamiliesGet(ILogger<FamiliesGet> logger, IMapper mapper, IUserRepository userRepository, IFamilyRepository familyRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _familyRepository = familyRepository;
            _mapper = mapper;
        }

        [FunctionName(nameof(FamiliesGet))]
        [RequiredScope("backendapi")]
        public async Task<IActionResult?> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "families")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var (authenticationStatus, authenticationResponse) = await req.HttpContext.AuthenticateAzureFunctionAsync();
            if (!authenticationStatus) return authenticationResponse;

            var userId = Guid.Parse(req.HttpContext.User.GetNameIdentifierId() ?? "");
            var families = await _familyRepository.GetFamiliesAsync(userId);

            var familieyQueryModels = families.Select(_mapper.Map<FamilyQueryModel>);

            var owners = GetOwnerNames(familieyQueryModels);
            foreach (var family in familieyQueryModels)
            {
                family.OwnerName = owners[family.OwnerId];
            }

            return new OkObjectResult(families);
        }


        private Dictionary<Guid, string> GetOwnerNames(IEnumerable<FamilyQueryModel> families)
        {
            // N+1 microservice problem... TODO solve by saving necessary information closer to usage or get list of users in one request
            var ownerIds = families.Select(s => s.OwnerId).Distinct();
            var tasks = new List<Task<User>>();
            foreach (var ownerId in ownerIds)
            {
                tasks.Add(_userRepository.GetUser(ownerId));
            }
            Task.WaitAll(tasks.ToArray());
            var owners = tasks.ToDictionary(k => k.Result.Id, v => v.Result.FullName);
            return owners;
        }
    }
}

