using EzDinner.Authorization.Core;
using NetCasbin;
using NetCasbin.Model;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace EzDinner.Infrastructure
{
    public class AuthzRepository : IAuthzRepository
    {
        private readonly Enforcer _enforcer;
        
        /// <summary>
        /// The PERM model that enables Casbin to enforce RBAC with domains authorization
        /// </summary>
        public const string RBAC_WITH_DOMAINS_MODEL =
@"[request_definition]
r = sub, dom, obj, act

[policy_definition]
p = sub, dom, obj, act

[role_definition]
g = _, _, _

[policy_effect]
e = some(where (p.eft == allow))

[matchers]
m = g(r.sub, p.sub, r.dom) && r.dom == p.dom && (r.obj == p.obj || p.obj == ""*"") && (r.act == p.act || p.act == ""*"")
";

        public AuthzRepository(Enforcer enforcer)
        {
            _enforcer = enforcer;
        }

        public Task AssignPolicyToRole(string role, Guid familyId, string resource, string action)
        {
            return _enforcer.AddPolicyAsync(role, familyId.ToString(), resource, action);
        }

        public bool RoleHasPolicy(string role, Guid familyId, string resource, string action)
        {
            return _enforcer.HasPolicy(role, familyId.ToString(), resource, action);
        }

        public Task AssignRoleToUser(Guid userId, Guid familyId, string role)
        {
            return _enforcer.AddRoleForUserAsync(userId.ToString(), role, familyId.ToString());
        }
      
        public bool UserHasRole(Guid userId, Guid familyId, string role)
        {
            return _enforcer.HasRoleForUser(userId.ToString(), role, familyId.ToString());
        }

        public bool VerifyUserPermission(string userId, string familyId, string resource, string action)
        {
            return _enforcer.Enforce(userId, familyId, resource, action);
        }
        public static Model GetRbacWithDomainsModel()
        {         
            return Model.CreateDefaultFromText(RBAC_WITH_DOMAINS_MODEL);
        }
    }
}
