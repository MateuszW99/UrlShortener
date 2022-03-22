using System.Linq;
using System.Text;
using UrlShortener.Application.Interfaces;

namespace UrlShortener.Application.Services
{
    public class UrlShortenerService : IUrlShortener
    {
        private static string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static int AlphabetLength = Alphabet.Length; 

        public int Decode(string url)
        {
            return url.Aggregate(0, (current, t) => current * AlphabetLength + Alphabet.IndexOf(t));
        }

        public string Encode(int encodedUrl)
        {
            if (encodedUrl < AlphabetLength)
            {
                return Alphabet[0].ToString();
            }
            
            var sb = new StringBuilder();
            
            while (encodedUrl > 0)
            {
                sb.Insert(0, Alphabet[encodedUrl % AlphabetLength]);
                encodedUrl /= AlphabetLength;
            }

            return sb.ToString();
        }
    }
}