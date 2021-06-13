using EzDinner.Authorization.Core;
using EzDinner.Core.Aggregates.FamilyAggregate;
using System.Threading.Tasks;

namespace EzDinner.Application.Commands.Authorization
{
    public class UpdateAuthorizationPoliciesCommand
    {
        private readonly IAuthzService _authz;

        public UpdateAuthorizationPoliciesCommand(IAuthzService authz)
        {
            _authz = authz;
        }

        public async Task Handle(Family family)
        {
            await _authz.CreateOwnerRolePermissionsAsync(family.Id);
            await _authz.CreateFamilyMemberRolePermissionsAsync(family.Id);

            // Ensure owner and members are assigned to their roles
            await _authz.AssignRoleToUserAsync(family.OwnerId, Roles.Owner, family.Id);
            foreach (var familyMemberId in family.FamilyMemberIds)
            {
                await _authz.AssignRoleToUserAsync(familyMemberId, Roles.FamilyMember, family.Id);
            }
        }
    }
}
