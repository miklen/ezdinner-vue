using AutoFixture;
using AutoFixture.Kernel;
using EzDinner.Core.Aggregates.DinnerAggregate;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EzDinner.UnitTests.DinnerTests
{
    public class DinnerServiceTests
    {
        private readonly Guid _familyId = Guid.NewGuid();
        private readonly DateTime _fromDate = new DateTime(2021, 1, 1);
        private readonly DateTime _toDate = new DateTime(2021, 1, 7);
        private readonly Mock<IDinnerRepository> _mockRepo;

        public DinnerServiceTests()
        {
            _mockRepo = new Mock<IDinnerRepository>();
        }

        [Fact]
        public async Task GetDinners_NoDinnersInRepo_FullRangeUnplannedReturned()
        {
            // Arrange
            _mockRepo.Setup(s => s.GetAsync(_familyId, _fromDate, _toDate)).Returns(Array.Empty<Dinner>().ToAsyncEnumerable());

            var sut = new DinnerService(_mockRepo.Object);

            // Act
            var dinners = await sut.GetAsync(_familyId, _fromDate, _toDate).ToListAsync();

            // Assert
            Assert.Equal(7, dinners.Count);
            Assert.Equal(_fromDate, dinners.First().Date);
            Assert.Equal(_toDate, dinners.Last().Date);
        }

        [Fact]
        public async Task GetDinners_FirstDinnerPlanned_FillUnplannedAfter()
        {
            // Arrange
            var fixture = CreateDinnerFixture();
            fixture.Inject(_fromDate);
            var dinner = fixture.Build<Dinner>()
                                .Create();

            _mockRepo.Setup(s => s.GetAsync(_familyId, _fromDate, _toDate)).Returns(new[] { dinner }.ToAsyncEnumerable());

            var sut = new DinnerService(_mockRepo.Object);

            // Act
            var dinners = await sut.GetAsync(_familyId, _fromDate, _toDate).ToListAsync();

            // Assert
            Assert.Equal(7, dinners.Count);
            Assert.Equal(dinner.Description, dinners.First().Description);
            Assert.Equal(dinner.Id, dinners.First().Id);
            Assert.Equal(_fromDate, dinners.First().Date);
            Assert.Equal(_toDate, dinners.Last().Date);
        }

        [Fact]
        public async Task GetDinners_LastDinnerPlanned_FillUnplannedBefore()
        {
            // Arrange
            var fixture = CreateDinnerFixture();
            fixture.Inject(_toDate);
            var dinner = fixture.Build<Dinner>()
                                .Create();

            _mockRepo.Setup(s => s.GetAsync(_familyId, _fromDate, _toDate)).Returns(new[] { dinner }.ToAsyncEnumerable());

            var sut = new DinnerService(_mockRepo.Object);

            // Act
            var dinners = await sut.GetAsync(_familyId, _fromDate, _toDate).ToListAsync();

            // Assert
            Assert.Equal(7, dinners.Count);
            Assert.Equal(dinner.Description, dinners.Last().Description);
            Assert.Equal(dinner.Id, dinners.Last().Id);
            Assert.Equal(_fromDate, dinners.First().Date);
            Assert.Equal(_toDate, dinners.Last().Date);
        }

        [Fact]
        public async Task GetDinners_MiddleOfWeekDinnerPlanned_FillUnplannedInterpolated()
        {
            // Arrange
            var fixture = CreateDinnerFixture();
            fixture.Inject(new DateTime(2021, 1, 4));
            var dinner = fixture.Build<Dinner>()
                                .Create();

            _mockRepo.Setup(s => s.GetAsync(_familyId, _fromDate, _toDate)).Returns(new[] { dinner }.ToAsyncEnumerable());

            var sut = new DinnerService(_mockRepo.Object);

            // Act
            var dinners = await sut.GetAsync(_familyId, _fromDate, _toDate).ToListAsync();

            // Assert
            Assert.Equal(7, dinners.Count);
            Assert.Equal(dinner.Description, dinners[3].Description);
            Assert.Equal(dinner.Id, dinners[3].Id);
            Assert.Equal(_fromDate, dinners.First().Date);
            Assert.Equal(_toDate, dinners.Last().Date);
        }

        [Fact]
        public async Task GetDinners_TwoDinnerPlanned_FillUnplannedInterpolated()
        {
            // Arrange
            var fixture = CreateDinnerFixture();
            fixture.Inject(new DateTime(2021, 1, 2));
            var dinner = fixture.Build<Dinner>()
                                .Create();
            
            fixture.Inject(new DateTime(2021, 1, 4));
            var dinner2 = fixture.Build<Dinner>()
                                .Create();

            _mockRepo.Setup(s => s.GetAsync(_familyId, _fromDate, _toDate)).Returns(new[] { dinner, dinner2 }.ToAsyncEnumerable());

            var sut = new DinnerService(_mockRepo.Object);

            // Act
            var dinners = await sut.GetAsync(_familyId, _fromDate, _toDate).ToListAsync();

            // Assert
            Assert.Equal(7, dinners.Count);
            Assert.Equal(dinner.Description, dinners[1].Description);
            Assert.Equal(dinner.Id, dinners[1].Id);
            Assert.Equal(dinner2.Description, dinners[3].Description);
            Assert.Equal(dinner2.Id, dinners[3].Id);
            Assert.Equal(_fromDate, dinners.First().Date);
            Assert.Equal(_toDate, dinners.Last().Date);
        }

        [Fact]
        public async Task GetDinners_AllDinnersPlanned_NoFill()
        {
            // Arrange
            var fixture = CreateDinnerFixture();
            _mockRepo.Setup(s => s.GetAsync(_familyId, _fromDate, _toDate)).Returns(CreateDinners(fixture, _fromDate, _toDate).ToAsyncEnumerable());

            var sut = new DinnerService(_mockRepo.Object);

            // Act
            var dinners = await sut.GetAsync(_familyId, _fromDate, _toDate).ToListAsync();

            // Assert
            Assert.Equal(7, dinners.Count);
            Assert.Empty(dinners.Where(w => string.IsNullOrEmpty(w.Description)));
            Assert.Equal(_fromDate, dinners.First().Date);
            Assert.Equal(_toDate, dinners.Last().Date);
        }

        private static Fixture CreateDinnerFixture()
        {
            var fixture = new Fixture();
            fixture.Customize<Dinner>(c => c.FromFactory(
                                                new MethodInvoker(
                                                    new GreedyConstructorQuery())));
            return fixture;
        }

        private IEnumerable<Dinner> CreateDinners(Fixture fixture, DateTime fromDate, DateTime toDate)
        {
            foreach(var day in EachDay(fromDate, toDate))
            {
                fixture.Inject(day);
                yield return fixture.Build<Dinner>().Create();
            }
        }

        private IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
            {
                yield return day;
            }
        }
    }
}
