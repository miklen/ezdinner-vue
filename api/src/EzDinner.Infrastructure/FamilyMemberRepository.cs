using EzDinner.Core.Aggregates.FamilyMemberAggregate;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzDinner.Infrastructure
{
    public class FamilyMemberRepository : IFamilyMemberRepository
    {
        private readonly GraphServiceClient _graphClient;

        public FamilyMemberRepository(GraphServiceClient graphClient)
        {
            _graphClient = graphClient;
        }

        public async Task<FamilyMember> GetFamilyMemberAsync(Guid id)
        {
            var user = await _graphClient.Users[id.ToString()].Request().GetAsync();
            // TODO use Automapper
            return new FamilyMember()
            {
                Id = id,
                GivenName = user.GivenName,
                FamilyName = user.Surname
            };
        }

        public async Task<FamilyMember?> GetFamilyMemberAsync(string email)
        {
            var userPage = await _graphClient.Users.Request().Filter($"eq(mail,'{email}')").GetAsync();
            var user = userPage.FirstOrDefault();
            if (user is null) return null;
            return new FamilyMember()
            {
                Id = Guid.Parse(user.Id),
                GivenName = user.GivenName,
                FamilyName = user.Surname
            };
        }
    }
}
