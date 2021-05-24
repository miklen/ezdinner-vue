using EzDinner.Core.Aggregates.FamilyAggregate;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EzDinner.UnitTests.FamilyTests
{
    public class FamilyTests
    {
        [Fact]
        public void Family_New_IsDeserializable()
        {
            // Arrange
            var family = Family.CreateNew(Guid.NewGuid(), "Test");
            family.InviteFamilyMember(Guid.NewGuid());

            // Act
            var json = JsonConvert.SerializeObject(family);
            var result = JsonConvert.DeserializeObject<Family>(json);

            // Assert
            Assert.Equal(family.Id, result.Id);
            Assert.Equal(family.OwnerId, result.OwnerId);
            Assert.Equal(family.Name, result.Name);
            Assert.Equal(family.FamilyMemberIds.First(), result.FamilyMemberIds.First());
            Assert.Equal(family.CreatedDate, result.CreatedDate);
            Assert.Equal(family.UpdatedDate, result.UpdatedDate);
        }
    }
}
