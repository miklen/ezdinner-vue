using EzDinner.Core.Aggregates.DinnerAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EzDinner.IntegrationTests.DinnerRepositoryTests
{
    public class DinnerRepositoryTests : IClassFixture<StartupFixture>
    {
        private IServiceProvider _provider;
        private IDinnerRepository _dinnerRepository;

        public DinnerRepositoryTests(StartupFixture startupFixture)
        {
            _provider = startupFixture.Provider;
            _dinnerRepository = (IDinnerRepository)_provider.GetService(typeof(IDinnerRepository));
        }

        [Fact]
        public async Task Get_DocumentExists_DinnerObjectReturned()
        {
            // Arrange
            var familyId = Guid.NewGuid();
            var date = DateTime.Now.Date;
            var dinner = new Dinner(familyId, date);
            await _dinnerRepository.SaveAsync(dinner);
            try
            {
                // Act
                var result = await _dinnerRepository.GetAsync(familyId, date);

                Assert.Equal(result.Id, dinner.Id);
            } finally
            {
                await _dinnerRepository.DeleteAsync(dinner);
            }
        }
    }
}
