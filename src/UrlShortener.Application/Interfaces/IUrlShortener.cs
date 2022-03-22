namespace UrlShortener.Application.Interfaces
{
    public interface IUrlShortener
    {
        int Decode(string url);
        string Encode(int encodedUrl);
    }
}