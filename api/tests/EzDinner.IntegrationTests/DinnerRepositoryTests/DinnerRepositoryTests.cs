using EzDinner.Core.Aggregates.DinnerAggregate;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var date = new LocalDate(2021, 1, 1);
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

        [Fact]
        public async Task GetMany_RangeExists_ObjectsWithInRangeReturned()
        {
            // Arrange
            var familyId = Guid.NewGuid();
            var dateMin = new LocalDate(2021, 1, 1);
            var dateMax = dateMin.PlusDays(1);
            var dinner1 = new Dinner(familyId, dateMin);
            var dinner2 = new Dinner(familyId, dateMax);
            await _dinnerRepository.SaveAsync(dinner1);
            await _dinnerRepository.SaveAsync(dinner2);
            try
            {
                // Act
                var result = new List<Dinner>();
                await foreach(var dinner  in _dinnerRepository.GetAsync(familyId, dateMin, dateMax))
                {
                    result.Add(dinner);
                }

                Assert.Equal(dinner1.Id, result.First().Id);
                Assert.Equal(dinner2.Id, result.Last().Id);
            }
            finally
            {
                await _dinnerRepository.DeleteAsync(dinner1);
                await _dinnerRepository.DeleteAsync(dinner2);
            }
        }
    }
}
