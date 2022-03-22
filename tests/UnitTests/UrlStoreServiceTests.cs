using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using UrlShortener.Application.Services;
using UrlShortener.Domain.Entities;
using UrlShortener.Persistence;
using Xunit;

namespace UnitTests
{
    public class UrlStoreServiceTests
    {
        private readonly UrlStoreService _sut;
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly int _lowestPossibleId;
        
        public UrlStoreServiceTests()
        {
            _lowestPossibleId = 1234;
            _contextMock = new Mock<IApplicationDbContext>();
            _sut = new UrlStoreService(_contextMock.Object);
        }

        [Fact]
        public async Task SaveShortenedUrl_Should_SaveNewEntity_and_SetIdToLowestPossibleValue()
        {
            var id = _lowestPossibleId; // default id provided in implementation
            var url = "https://stackoverflow.com/";
            
            var urlShort = new UrlShort { Id = id, FullUrl = url };
            var urlShorts = new List<UrlShort> { urlShort };
            var mockedUrlShorts = urlShorts.AsQueryable().BuildMockDbSet();

            var emptyUrlShortsList = new List<UrlShort>();
            var mockedEmptyUrlShortsList = emptyUrlShortsList.AsQueryable().BuildMockDbSet();

            _contextMock
                .Setup(x => x.UrlShorts)
                .Returns(mockedEmptyUrlShortsList.Object);
            
            _contextMock
                .Setup(x => x.UrlShorts.AddAsync(It.IsAny<UrlShort>(), It.IsAny<CancellationToken>()))
                .Callback(() =>
                    _contextMock
                        .Setup(x => x.UrlShorts)
                        .Returns(mockedUrlShorts.Object)
                );

            await _sut.SaveShortenedUrl(url);

            var entityList = await _contextMock.Object.UrlShorts.ToListAsync();
            entityList.Count.Should().Be(1);
            entityList.First().Id.Should().Be(id);
            entityList.First().FullUrl.Should().Be(url);
        }
            
        [Fact]
        public async Task SaveShortenedUrl_Should_SaveNewEntity_and_SetIdToNextPossibleValue()
        {
            var id = _lowestPossibleId; // default id provided in implementation
            var url = "https://stackoverflow.com/";
            
            var urlShort1 = new UrlShort { Id = id, FullUrl = url };
            var urlShort2 = new UrlShort { Id = id + 1, FullUrl = url };
            var urlShortsBefore = new List<UrlShort> { urlShort1 };
            var mockedUrlShortsListBefore = urlShortsBefore.AsQueryable().BuildMockDbSet();

            var urlShortsListAfter = new List<UrlShort> { urlShort1, urlShort2 };
            var mockedUrlShortsListAfter = urlShortsListAfter.AsQueryable().BuildMockDbSet();

            _contextMock
                .Setup(x => x.UrlShorts)
                .Returns(mockedUrlShortsListBefore.Object);
            
            _contextMock
                .Setup(x => x.UrlShorts.AddAsync(It.IsAny<UrlShort>(), It.IsAny<CancellationToken>()))
                .Callback(() =>
                    _contextMock
                        .Setup(x => x.UrlShorts)
                        .Returns(mockedUrlShortsListAfter.Object)
                );

            await _sut.SaveShortenedUrl(url);

            var entityList = await _contextMock.Object.UrlShorts.ToListAsync();
            entityList.Count.Should().Be(mockedUrlShortsListAfter.Object.Count());
            entityList.First().Id.Should().Be(id);
            entityList.First().FullUrl.Should().Be(url);
        }
        
        [Fact]
        public async Task GetFullUrl_Should_ReturnData_When_DatabaseIsNotEmpty()
        {
            var id = _lowestPossibleId;
            var url = "https://stackoverflow.com/";
            var urlShort = new UrlShort { Id = id, FullUrl = url };
            var urlShorts = new List<UrlShort> { urlShort };
            var mockedUrlShorts = urlShorts.AsQueryable().BuildMockDbSet();

            _contextMock
                .Setup(x => x.UrlShorts)
                .Returns(mockedUrlShorts.Object);

            var result = await _sut.GetFullUrl(id);

            result.Should().Be(url);
        }
        
        [Fact]
        public async Task GetFullUrl_Should_ReturnNull_When_DatabaseIsEmpty()
        {
            var id = _lowestPossibleId;
            var urlShorts = new List<UrlShort>();
            var mockedUrlShorts = urlShorts.AsQueryable().BuildMockDbSet();

            _contextMock
                .Setup(x => x.UrlShorts)
                .Returns(mockedUrlShorts.Object);

            var result = await _sut.GetFullUrl(id);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllShortenedUrls_Should_ReturnData_When_DatabaseIsNotEmpty()
        {
            var urlShort1 = new UrlShort { Id = _lowestPossibleId, FullUrl = "https://stackoverflow.com/" };
            var urlShort2 = new UrlShort { Id = _lowestPossibleId + 1, FullUrl = "https://stackoverflow.com/" };
            var urlShorts = new List<UrlShort> { urlShort1, urlShort2 };
            var mockedUrlShorts = urlShorts.AsQueryable().BuildMockDbSet();

            _contextMock
                .Setup(x => x.UrlShorts)
                .Returns(mockedUrlShorts.Object);

            var result = await _sut.GetAllShortenedUrls();

            result.Count.Should().Be(urlShorts.Count);
        }

        [Fact]
        public async Task GetAllShortenedUrls_Should_ReturnEmptyList_When_DatabaseIsEmpty()
        {
            var urlShorts = new List<UrlShort>();
            var mockedUrlShorts = urlShorts.AsQueryable().BuildMockDbSet();

            _contextMock
                .Setup(x => x.UrlShorts)
                .Returns(mockedUrlShorts.Object);

            var result = await _sut.GetAllShortenedUrls();

            result.Count.Should().Be(urlShorts.Count);
        }

        [Fact]
        public async Task GetNextPossibleId_Should_ReturnLowestPossibleId_When_DatabaseIsEmpty()
        {
            var urlShorts = new List<UrlShort>();
            var mockedUrlShorts = urlShorts.AsQueryable().BuildMockDbSet();

            _contextMock
                .Setup(x => x.UrlShorts)
                .Returns(mockedUrlShorts.Object);

            var result = await _sut.GetNextPossibleId();

            result.Should().Be(_lowestPossibleId);
        }
        
        
        [Fact]
        public async Task GetNextPossibleId_Should_ReturnNextPossibleId_When_DatabaseIsNotEmpty()
        {
            var urlShorts = new List<UrlShort> { new() { Id = _lowestPossibleId, FullUrl = "https://stackoverflow.com/" } };
            var mockedUrlShorts = urlShorts.AsQueryable().BuildMockDbSet();

            _contextMock
                .Setup(x => x.UrlShorts)
                .Returns(mockedUrlShorts.Object);

            var result = await _sut.GetNextPossibleId();

            result.Should().Be(_lowestPossibleId + 1);
        }
    }
}