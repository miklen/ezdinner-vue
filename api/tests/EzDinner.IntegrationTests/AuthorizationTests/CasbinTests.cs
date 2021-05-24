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
        private Enforcer _enforcer;

        public CasbinTests(StartupFixture startupFixture)
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
m = (g(r.sub, p.sub, r.dom) && r.dom == p.dom && r.obj == p.obj && r.act == p.act) || (g(r.sub, p.sub, r.dom) && p.sub == ""Owner"")
";

            var model = Model.CreateDefaultFromText(modelString);
            _enforcer = new Enforcer(model, (CasbinCosmosAdapater)startupFixture.Provider.GetService(typeof(CasbinCosmosAdapater)));
        }
        
        [Fact]
        public async Task Enforcer_NoRoles_CanAddRole()
        {
            // Arrange
            var role = "Owner";
            var id = Guid.NewGuid().ToString();
            try
            {
                // Act
                await _enforcer.AddPolicyAsync(role, id);

                // Assert
                var exists = _enforcer.HasPolicy(role, id);
                Assert.True(exists);
            } finally
            {
                await _enforcer.RemovePolicyAsync(role, id);
            }
        }
    }
}
