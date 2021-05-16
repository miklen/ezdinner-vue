using EzDinner.Core.Aggregates.UserAggregate;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzDinner.Infrastructure
{
    public class FamilyMemberRepository : IUserRepository
    {
        private readonly GraphServiceClient _graphClient;

        public FamilyMemberRepository(GraphServiceClient graphClient)
        {
            _graphClient = graphClient;
        }

        public async Task<Core.Aggregates.UserAggregate.User> GetUser(Guid id)
        {
            var user = await _graphClient.Users[id.ToString()].Request().GetAsync();
            // TODO use Automapper
            return new Core.Aggregates.UserAggregate.User()
            {
                Id = id,
                GivenName = user.GivenName,
                FamilyName = user.Surname
            };
        }

        public async Task<Core.Aggregates.UserAggregate.User?> GetUser(string email)
        {
            var userPage = await _graphClient.Users.Request().Filter($"eq(mail,'{email}')").GetAsync();
            var user = userPage.FirstOrDefault();
            if (user is null) return null;
            return new Core.Aggregates.UserAggregate.User()
            {
                Id = Guid.Parse(user.Id),
                GivenName = user.GivenName,
                FamilyName = user.Surname
            };
        }
    }
}
