using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Identity.Web;
using EzDinner.Core.Aggregates.FamilyMemberAggregate;
using EzDinner.Core.Aggregates.FamilyAggregate;
using EzDinner.Functions.Models.Command;

namespace EzDinner.Functions
{
    public class FamilyInviteMember
    {
        private readonly ILogger<FamilyInviteMember> _logger;
        private readonly IFamilyMemberRepository _familyMemberRepository;
        private readonly IFamilyRepository _familyRepository;

        public FamilyInviteMember(ILogger<FamilyInviteMember> logger, IFamilyMemberRepository familyMemberRepository, IFamilyRepository familyRepository)
        {
            _logger = logger;
            _familyMemberRepository = familyMemberRepository;
            _familyRepository = familyRepository;
        }
        
        [FunctionName(nameof(FamilyInviteMember))]
        public async Task<IActionResult?> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "family/{familyId}/member")] HttpRequest req,
            string familyId)
        {

            var (authenticationStatus, authenticationResponse) = await req.HttpContext.AuthenticateAzureFunctionAsync();
            if (!authenticationStatus) return authenticationResponse;

            // TODO: Refactor to FamilyService - responsibilty is to handle cross-aggregate logic
            var familyGuid = Guid.Parse(familyId);
            if (familyGuid.Equals(Guid.Empty)) return new BadRequestObjectResult("MISSING_FAMILYID");
            
            var command = await req.GetBodyAs<InviteFamilyMemberCommandModel>();
            if (string.IsNullOrEmpty(command.Email)) return new BadRequestObjectResult("MISSING_EMAIL");

            var familyMember = await _familyMemberRepository.GetFamilyMemberAsync(command.Email);
            if (familyMember is null) return new NoContentResult();

            var family = await _familyRepository.GetFamily(familyGuid);
            if (family is null) return new BadRequestObjectResult("NOT_FOUND_FAMILY");

            family.InviteFamilyMember(familyMember.Id);
            return new OkResult();
        }
    }
}
