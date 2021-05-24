using Casbin.Adapter.EFCore;
using EzDinner.Infrastructure;
using NetCasbin;
using NetCasbin.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EzDinner.IntegrationTests.AuthorizationTests
{
    public class CasbinTests : IClassFixture<StartupFixture>
    {
        private Model _model;
        private IServiceProvider _provider;
        private Enforcer _enforcer;
        private Enforcer Enforcer => _enforcer ??= new Enforcer(_model, (CasbinCosmosAdapter)_provider.GetService(typeof(CasbinCosmosAdapter)));

        public CasbinTests(StartupFixture startupFixture)
        {
            _provider = startupFixture.Provider;
            CreateModel();
        }

        private void CreateModel()
        {
            var modelString =
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
            _model = Model.CreateDefaultFromText(modelString);
        }

        [Fact]
        public void Context_NoDatabase_CanCreate()
        {
            var context = (CasbinDbContext<Guid>)_provider.GetService(typeof(CasbinDbContext<Guid>));
            context.Database.EnsureCreated();
        }

        [Fact]
        public async Task Enforcer_NoRoles_CanAddRole()
        {
            // Arrange
            var roleDefinitionPolicy = CreateOwnerRolePolicy(Guid.NewGuid());
            try
            {
                // Act
                await Enforcer.AddPolicyAsync(roleDefinitionPolicy);

                // Assert
                var exists = Enforcer.HasPolicy(roleDefinitionPolicy);
                Assert.True(exists);
            }
            finally
            {
                await Enforcer.RemovePolicyAsync(roleDefinitionPolicy);
            }
        }



        [Fact]
        public async Task Enforcer_NoUsersInRole_CanAddUserToRole()
        {
            // Arrange
            var roleAssignmentPolicy = CreateRoleAssignmentPolicy(Guid.NewGuid(), Guid.NewGuid(), "Owner") ;
            try
            {
                // Act
                await Enforcer.AddGroupingPolicyAsync(roleAssignmentPolicy);

                // Assert
                var exists = Enforcer.HasGroupingPolicy(roleAssignmentPolicy);
                Assert.True(exists);
            }
            finally
            {
                await Enforcer.RemoveGroupingPolicyAsync(roleAssignmentPolicy);
            }
        }


        [Fact]
        public async Task Enforcer_UserAssignedToOwnerRole_AnyActionAllowedInDomain()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var domainId = Guid.NewGuid();
            var roleDefinitionPolicy = CreateOwnerRolePolicy(domainId);
            var roleAssignmentPolicy = CreateRoleAssignmentPolicy(userId, domainId, "Owner");
            try
            {
                await Enforcer.AddPolicyAsync(roleDefinitionPolicy);
                await Enforcer.AddGroupingPolicyAsync(roleAssignmentPolicy);

                // Act
                var result1 = await Enforcer.EnforceAsync(userId.ToString(), domainId.ToString(), "Permission.Family", "Update");
                var result2 = await Enforcer.EnforceAsync(userId.ToString(), domainId.ToString(), "Something.Random", "RandomAction");

                // Assert
                Assert.True(result1);
                Assert.True(result2);
            }
            finally
            {
                await Enforcer.RemoveGroupingPolicyAsync(roleAssignmentPolicy);
                await Enforcer.RemovePolicyAsync(roleDefinitionPolicy);
            }
        }

        [Fact]
        public async Task Enforcer_UserAssignedToOwnerRole_NotAllowedActionsInOtherDomains()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var domainId = Guid.NewGuid();
            var roleDefinitionPolicy = CreateOwnerRolePolicy(domainId);
            var roleAssignmentPolicy = CreateRoleAssignmentPolicy(userId, domainId, "Owner");
            var otherDomain = Guid.NewGuid().ToString();
            try
            {
                await Enforcer.AddPolicyAsync(roleDefinitionPolicy);
                await Enforcer.AddGroupingPolicyAsync(roleAssignmentPolicy);

                // Act
                var result = await Enforcer.EnforceAsync(userId.ToString(), otherDomain, "Permission.Family", "Update");
                
                // Assert
                Assert.False(result);
            }
            finally
            {
                await Enforcer.RemoveGroupingPolicyAsync(roleAssignmentPolicy);
                await Enforcer.RemovePolicyAsync(roleDefinitionPolicy);
            }
        }


        private static string[] CreateRoleAssignmentPolicy(Guid userId, Guid domainId, string role)
        {
            var rolePolicy = new[] { userId.ToString(), role, domainId.ToString() };
            return rolePolicy;
        }
        private static string[] CreateOwnerRolePolicy(Guid domainId)
        {
            var role = "Owner";
            var permission = "*";
            var action = "*";
            return CreateRolePolicy(role, domainId, permission, action);
        }

        private static string[] CreateRolePolicy(string role, Guid domainId, string permission, string action)
        {
            var policy = new[] { role, domainId.ToString(), permission, action };
            return policy;
        }
    }
}
