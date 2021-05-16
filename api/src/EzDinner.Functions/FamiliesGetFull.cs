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
    public class FamiliesGetFull
    {
        private readonly ILogger<FamiliesGet> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IFamilyRepository _familyRepository;
        private readonly IMapper _mapper;

        public FamiliesGetFull(ILogger<FamiliesGet> logger, IMapper mapper, IUserRepository userRepository, IFamilyRepository familyRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _familyRepository = familyRepository;
            _mapper = mapper;
        }

        [FunctionName(nameof(FamiliesGetFull))]
        [RequiredScope("backendapi")]
        public async Task<IActionResult?> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "families")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var (authenticationStatus, authenticationResponse) = await req.HttpContext.AuthenticateAzureFunctionAsync();
            if (!authenticationStatus) return authenticationResponse;

            var familyResult = await GetFamilies(req);

            return new OkObjectResult(familyResult);
        }

        private async Task<List<FamilyQueryModel>> GetFamilies(HttpRequest req)
        {
            var userId = Guid.Parse(req.HttpContext.User.GetNameIdentifierId() ?? "");
            var families = await _familyRepository.getFamilySelectorsAsync(userId);

            var familyMembers = GetUserNames(families);
            var familyResult = new List<FamilyQueryModel>();
            foreach (var family in families)
            {
                var familyQueryModel = _mapper.Map<FamilyQueryModel>(family);
                familyQueryModel.FamilyMembers.Add(new FamilyMemberQueryModel(family.OwnerId, familyMembers[family.OwnerId], true));
                foreach (var familyMemberId in family.FamilyMemberIds)
                {
                    familyQueryModel.FamilyMembers.Add(new FamilyMemberQueryModel(familyMemberId, familyMembers[familyMemberId]));
                }
                familyResult.Add(familyQueryModel);
            }

            return familyResult;
        }

        private Dictionary<Guid, string> GetUserNames(IEnumerable<Family> families)
        {
            // N+1 microservice problem... TODO solve by saving necessary information closer to usage or get list of users in one request
            var ownerIds = families.Select(s => s.OwnerId).Distinct();
            var memberIds = families.SelectMany(s => s.FamilyMemberIds).Distinct();
            var tasks = new List<Task<User>>();
            foreach (var userId in ownerIds.Union(memberIds))
            {
                tasks.Add(_userRepository.GetUser(userId));
            }
            Task.WaitAll(tasks.ToArray());
            var users = tasks.ToDictionary(k => k.Result.Id, v => v.Result.FullName);
            return users;
        }
    }
}

