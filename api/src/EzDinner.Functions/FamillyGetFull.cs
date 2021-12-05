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
        private readonly IUserRepository _userRepository;
        private readonly IFamilyRepository _familyRepository;
        private readonly IMapper _mapper;
        private readonly IAuthzRepository _authz;

        public FamilyGetFull(ILogger<FamilyGetFull> logger, IMapper mapper, IAuthzRepository authz, IUserRepository userRepository, IFamilyRepository familyRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _familyRepository = familyRepository;
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
            var family = await _familyRepository.GetFamily(Guid.Parse(familyId));
            if (family is null) return new BadRequestObjectResult("Family not found");
            var familyResult = await GetFamily(family);
            return new OkObjectResult(familyResult);
        }

        private Task<FamilyQueryModel> GetFamily(Family family)
        {
            var familyMembers = GetUserNames(family);
            var familyQueryModel = _mapper.Map<FamilyQueryModel>(family);
            familyQueryModel.FamilyMembers.Add(new FamilyMemberQueryModel(family.OwnerId, familyMembers[family.OwnerId], true));
            foreach (var familyMemberId in family.FamilyMemberIds)
            {
                familyQueryModel.FamilyMembers.Add(new FamilyMemberQueryModel(familyMemberId, familyMembers[familyMemberId]));
            }

            return Task.FromResult(familyQueryModel);
        }

        private Dictionary<Guid, string> GetUserNames(Family? family)
        {
            // N+1 microservice problem... TODO solve by saving necessary information closer to usage or get list of users in one request
            var ownerIds = new List<Guid>() { family.OwnerId };
            var memberIds = family.FamilyMemberIds.Select(s => s).Distinct();
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

