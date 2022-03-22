## UrlShortener API

### How does it work?
Just provide a valid url to POST `api/urls/{url}`. You will receive a short string. To get a redirect with your short string, just send a request to GET `api/urls/{yourShortenedUrl}`.

You can also see all shortened URLs from in-memory database by sending requests to GET `api/urls`.

## How to run it?
The solution is built on .NET 5.0. Install runtime on your machine and then build the solution. No database provider is required as it uses in-memory database. 

Note: when launching the app, swagger docs appear. Swagger doesn't support redirects to other pages, so to be redirected, put a shortened url in your browser.