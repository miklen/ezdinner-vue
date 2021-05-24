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
            var role = "Owner";
            var id = Guid.NewGuid().ToString();
            try
            {
                // Act
                await Enforcer.AddPolicyAsync(role, id);

                // Assert
                var exists = Enforcer.HasPolicy(role, id);
                Assert.True(exists);
            } finally
            {
                await Enforcer.RemovePolicyAsync(role, id);
            }
        }
    }
}
