using Casbin.Adapter.EFCore;
using Casbin.Adapter.EFCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Infrastructure
{
    public class CasbinCosmosAdapter : EFCoreAdapter<string>
    {
        public CasbinCosmosAdapter(CasbinDbContext<string> casbinDbContext)
            : base(casbinDbContext)
        {
        }

        // This method will be called before calling `DbCotnext.CasbinRules.Add(casbinRule);`
        protected override CasbinRule<string> OnAddPolicy(string section, string policyType, IEnumerable<string> rule, CasbinRule<string> casbinRule)
        {
            // Generating and set the Id property.
            casbinRule.Id = Guid.NewGuid().ToString();
            return base.OnAddPolicy(section, policyType, rule, casbinRule);
        }
    }
}
