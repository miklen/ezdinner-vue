using Casbin.Adapter.EFCore;
using Casbin.Adapter.EFCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Infrastructure
{
    public class CasbinEntityConfiguration : DefaultCasbinRuleEntityTypeConfiguration<string>
    {
        private const string _containerName = "CasbinRules";
        
        public CasbinEntityConfiguration() : base(_containerName) {}

        public override void Configure(EntityTypeBuilder<CasbinRule<string>> builder)
        {
            base.Configure(builder);
            builder.ToContainer(_containerName);
            builder.HasPartitionKey(p => p.Id);
        }
    }
}
