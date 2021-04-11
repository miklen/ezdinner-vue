using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using EzDinner.Core.Aggregates.FamilyAggregate;
using EzDinner.Core.Aggregates.FamilyMemberAggregate;
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
    public class GetFamilies
    {
        private readonly ILogger<GetFamilies> _logger;
        private readonly IFamilyMemberRepository _familyMemberRepository;
        private readonly IFamilyRepository _familyRepository;

        public GetFamilies(ILogger<GetFamilies> logger, IFamilyMemberRepository familyMemberRepository, IFamilyRepository familyRepository)
        {
            _logger = logger;
            _familyMemberRepository = familyMemberRepository;
            _familyRepository = familyRepository;
        }

        [FunctionName("families")]
        [RequiredScope("backendapi")] // The Azure Function will only accept tokens 1) for users, and 2) having the "access_as_user" scope for this API
        public async Task<IActionResult?> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var (authenticationStatus, authenticationResponse) = await req.HttpContext.AuthenticateAzureFunctionAsync();
            if (!authenticationStatus) return authenticationResponse;

            var userId = Guid.Parse(req.HttpContext.User.GetNameIdentifierId() ?? "");
            var families = await _familyRepository.GetFamiliesAsync(userId);

            var familieyQueryModels = families.Select(familiy => new FamilyQueryModel() { Id = familiy.Id, OwnerId = familiy.OwnerId, Name = familiy.Name });

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
            var tasks = new List<Task<FamilyMember>>();
            foreach (var ownerId in ownerIds)
            {
                tasks.Add(_familyMemberRepository.GetFamilyMemberAsync(ownerId));
            }
            Task.WaitAll(tasks.ToArray());
            var owners = tasks.ToDictionary(k => k.Result.Id, v => v.Result.FullName);
            return owners;
        }
    }
}

