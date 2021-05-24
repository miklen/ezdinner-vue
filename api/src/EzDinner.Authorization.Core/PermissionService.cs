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

        /// <summary>
        /// Role definitions:
        /// p, admin, domain1, data1, read
        /// p, admin, domain1, data1, write
        /// p, admin, domain2, data2, read
        /// p, admin, domain2, data2, write
        /// p, owner, domain2, *, *
        /// p, all_data2, domain2, data2, *
        /// p, all_read, domain2, *, read
        ///
        /// User assignment to roles:
        /// g, alice, admin, domain1
        /// g, bob, admin, domain2
        /// g, mikkel, owner, domain2
        /// g, foo, all_data2, domain2
        /// g, bar, all_read, domain2
        /// 
        /// Enforcements:
        /// alice, domain1, data2, read = false
        /// alice, domain2, data3, read = false
        /// mikkel, domain2, data2, write = true
        /// mikkel, domain2, data3, write = true
        /// mikkel, domain1, data1, write = false
        /// bob, domain2, data3, write = false
        /// bob, domain2, data2, write = true
        /// foo, domain2, data2, read = true
        /// foo, domain2, data2, write = true
        /// foo, domain2, data1, read = false
        /// foo, domain1, data2, write = false
        /// bar, domain2, data1, read = true
        /// bar, domain2, data1, write = false
        /// bar, domain2, data2, read = true
        /// </summary>
        /// <param name="enforcer"></param>
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
