namespace It4see.IntegrationTests.TestSuite
{
    public class HttpClientFactory
    {
        public static HttpClient Create(WebAppFactory factory)
        {
            var httpClient = factory.CreateClient();
            httpClient.BaseAddress = new Uri("https://localhost/");

            return httpClient;
        }
    }
}
