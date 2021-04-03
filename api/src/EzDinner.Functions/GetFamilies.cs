using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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

        public GetFamilies(ILogger<GetFamilies> logger, IFamilyMemberRepository familyMemberRepository)
        {
            _logger = logger;
            _familyMemberRepository = familyMemberRepository;
        }

        [FunctionName("Families")]
        [RequiredScope("backendapi")] // The Azure Function will only accept tokens 1) for users, and 2) having the "access_as_user" scope for this API
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var (authenticationStatus, authenticationResponse) = await req.HttpContext.AuthenticateAzureFunctionAsync();
            if (!authenticationStatus) return authenticationResponse;

            // TODO replace with getting the families that a user is a member of
            var families = new List<FamilyQueryModel>() { new FamilyQueryModel() { Id = Guid.NewGuid(), Name = "Nygaard", OwnerId = Guid.Parse("211c4ea8-f8cb-49df-9b04-75d9669d035e") }, new FamilyQueryModel() { Id = Guid.NewGuid(), Name = "Bengtson", OwnerId = Guid.Parse("211c4ea8-f8cb-49df-9b04-75d9669d035e") } };

            var owners = GetOwnerNames(families);
            foreach (var family in families)
            {
                family.OwnerName = owners[family.OwnerId];
            }

            return new OkObjectResult(families);
        }

        private Dictionary<Guid, string> GetOwnerNames(List<FamilyQueryModel> families)
        {
            // N+1 microservice problem... TODO solve by saving necessary information closer to usage or get list of users in one request
            var ownerIds = families.Select(s => s.OwnerId).Distinct();
            var tasks = new List<Task<FamilyMember>>();
            foreach (var ownerId in ownerIds)
            {
                tasks.Add(_familyMemberRepository.GetFamilyMember(ownerId));
            }
            Task.WaitAll(tasks.ToArray());
            var owners = tasks.ToDictionary(k => k.Result.Id, v => v.Result.FullName);
            return owners;
        }
    }
}

