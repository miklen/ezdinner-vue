using EzDinner.Core.Aggregates.FamilyMemberAggregate;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
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
    }
}
