using AutoMapper;
using Application.Schemas.Profiles;
using Application.Schemas.Requests;
using Application.Services.Implementations;
using Domain.Entities;
using Domain.Repository;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests
{
    public class TowerServiceTests
    {
        private readonly IMapper _mapper;

        public TowerServiceTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<TowerProfile>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task GetPublicTowerListAsync_FiltersAndPaginates()
        {
            // Arrange
            var towers = new List<Tower>
            {
                new Tower { Id = 1, Name = "Alpha" },
                new Tower { Id = 2, Name = "Beta" },
                new Tower { Id = 3, Name = "Alpine" }
            }.AsQueryable();

            var repoMock = new Mock<ITowerRepository>();
            repoMock.Setup(r => r.GetAsQueryable()).Returns(towers);
            var service = new TowerService(repoMock.Object, _mapper);

            var filter = new TowerFilterParams
            {
                Name = "Al",
                PageNumber = 1,
                PageSize = 1,
                SortBy = "Name",
                SortOrder = "asc"
            };

            // Act
            var result = await service.GetPublicTowerListAsync(filter);

            // Assert
            Assert.Equal(2, result.TotalRecords);
            Assert.Single(result.Data);
            Assert.Equal("Alpha", result.Data.First().Name);
        }

        [Fact]
        public async Task GetPublicTowerListAsync_ReturnsSecondPage()
        {
            // Arrange
            var towers = new List<Tower>
            {
                new Tower { Id = 1, Name = "Alpha" },
                new Tower { Id = 2, Name = "Beta" },
                new Tower { Id = 3, Name = "Alpine" }
            }.AsQueryable();

            var repoMock = new Mock<ITowerRepository>();
            repoMock.Setup(r => r.GetAsQueryable()).Returns(towers);
            var service = new TowerService(repoMock.Object, _mapper);

            var filter = new TowerFilterParams
            {
                Name = "Al",
                PageNumber = 2,
                PageSize = 1,
                SortBy = "Name",
                SortOrder = "asc"
            };

            // Act
            var result = await service.GetPublicTowerListAsync(filter);

            // Assert
            Assert.Single(result.Data);
            Assert.Equal("Alpine", result.Data.First().Name);
        }
    }
}
