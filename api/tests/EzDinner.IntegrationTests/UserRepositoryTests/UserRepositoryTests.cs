using EzDinner.Core.Aggregates.UserAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EzDinner.IntegrationTests.UserRepositoryTests
{
    public class UserRepositoryTests : IClassFixture<StartupFixture>
    {
        private IUserRepository _userRepository;

        public UserRepositoryTests(StartupFixture startup)
        {
            _userRepository = (IUserRepository)startup.Provider.GetService(typeof(IUserRepository));
        }

        [Fact]
        public async Task GetByMail_UserExists_ReturnsDomainUser()
        {
            var user = await _userRepository.GetUser("mikkel@bnyg.dk");

            Assert.NotNull(user);
            Assert.Equal("Mikkel", user.GivenName);
        }
    }
}
