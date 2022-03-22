using FluentAssertions;
using UrlShortener.Application.Services;
using Xunit;

namespace UnitTests
{
    public class UrlShortenerServiceTests
    {
        private readonly UrlShortenerService _sut;
        private readonly int _decoded;
        private readonly string _encoded;
        
        public UrlShortenerServiceTests()
        {
            _sut = new UrlShortenerService();
            _decoded = 1212694755;
            _encoded = "n0cap";
        }

        [Fact]
        public void Encode_and_Decode_Should_Work_Bijectly()
        {
            var encodedResult = _sut.Encode(_decoded);
            var decodedResult = _sut.Decode(encodedResult);

            decodedResult.Should().Be(_decoded);
        }

        [Fact]
        public void Decode_and_Encode_Should_Work_Bijectly()
        {
            var decodedResult = _sut.Decode(_encoded);
            var encodedResult = _sut.Encode(decodedResult);

            encodedResult.Should().Be(_encoded);
        }
    }
}