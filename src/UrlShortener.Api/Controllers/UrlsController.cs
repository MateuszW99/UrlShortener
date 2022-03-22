using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UrlShortener.Application.Interfaces;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlsController : ControllerBase
    {
        private readonly IUrlShortener _urlShortener;
        private readonly IUrlStoreService _urlStoreService;
        private readonly ILogger<UrlsController> _logger;
        
        public UrlsController(
            IUrlShortener urlShortener,
            IUrlStoreService urlStoreService,
            ILogger<UrlsController> logger)
        {
            _urlShortener = urlShortener;
            _urlStoreService = urlStoreService;
            _logger = logger;
        }

        [HttpGet("{shortUrl}")]
        public async Task<IActionResult> RedirectToFullUrl(string shortUrl)
        {
            LogRequest("RedirectToFullUrl", shortUrl);
            var id = _urlShortener.Decode(shortUrl);
            var urlRecord = await _urlStoreService.GetFullUrl(id);

            return Redirect(urlRecord);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRecords()
        {
            LogRequest("GetAllRecords", null);
            var records = await _urlStoreService.GetAllShortenedUrls();
            
            return Ok(records);
        }

        [HttpPost]
        public async Task<IActionResult> CreateShortUrl([Url] string fullUrl)
        {
            LogRequest("CreateShortUrl", fullUrl);
            var id = await _urlStoreService.SaveShortenedUrl(fullUrl);
            var shortenedUrl = _urlShortener.Encode(id);
            return Ok(new { ShortenedUrl = shortenedUrl });
        }

        private void LogRequest(string requestName, string? resource)
        {
            var sb = new StringBuilder();
            
            var timestamp = DateTime.Now;
            sb.Append($"Request received at {timestamp}{Environment.NewLine}RequestName = {requestName}");
            if (!string.IsNullOrEmpty(resource))
            {
                sb.Append($"{Environment.NewLine}RequestResource = {resource}");    
            }
            _logger.LogInformation(sb.ToString());
        }
    }
}