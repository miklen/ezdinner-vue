using NetCasbin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EzDinner.Authorization
{
    public class PermissionService : IPermissionService
    {
        private readonly Enforcer _enforcer;

        public PermissionService(Enforcer enforcer)
        {
            _enforcer = enforcer;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="role">A role defined in <see cref="Roles"/></param>
        /// <param name="familyId"></param>
        /// <returns></returns>
        public Task AssignRoleToUser(Guid userId, string role, Guid familyId)
        {
            if (!_enforcer.HasRoleForUser(userId.ToString(), role, familyId.ToString()))
            {
                // Create Owner role for the created famiy
                return _enforcer.AddRoleForUserAsync(userId.ToString(), role, familyId.ToString());
            }
            return Task.CompletedTask;
        }

        public Task CreateOwnerRole(Guid familyId)
        {
            if (!_enforcer.HasPolicy(Roles.Owner, familyId.ToString(), Resources.All, Actions.All))
            {
                // Assign Owner role to the user who created the family
                return _enforcer.AddPolicyAsync(Roles.Owner, familyId.ToString(), Resources.All, Actions.All);
            }
            return Task.CompletedTask;
        }
    }
}
