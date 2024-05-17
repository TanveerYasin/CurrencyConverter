using Newtonsoft.Json.Linq;
namespace CurrencyConverterApi.ApiServices
{
    public class CurrencyService : ICurrencyService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://api.frankfurter.app/";

        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<JObject> GetLatestRatesAsync(string baseCurrency)
        {
            var response = await RetryPolicy.ExecuteAsync(() => _httpClient.GetAsync($"{BaseUrl}latest?base={baseCurrency}"));
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JObject.Parse(content);
        }

        public async Task<JObject> ConvertAmountAsync(string from, string to, decimal amount)
        {
            var response = await RetryPolicy.ExecuteAsync(() => _httpClient.GetAsync($"{BaseUrl}latest?base={from}"));
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var rates = JObject.Parse(content)["rates"];
            var rate = rates[to].Value<decimal>();
            return new JObject { ["amount"] = amount * rate };
        }

        public async Task<JObject> GetHistoricalRatesAsync(string baseCurrency, DateTime startDate, DateTime endDate)
        {
            var response = await RetryPolicy.ExecuteAsync(() => _httpClient.GetAsync($"{BaseUrl}{startDate:yyyy-MM-dd}..{endDate:yyyy-MM-dd}?base={baseCurrency}"));
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JObject.Parse(content);
        }

        private static class RetryPolicy
        {
            public static async Task<HttpResponseMessage> ExecuteAsync(Func<Task<HttpResponseMessage>> action, int retries = 3)
            {
                for (int attempt = 1; attempt <= retries; attempt++)
                {
                    var response = await action();
                    if (response.IsSuccessStatusCode)
                    {
                        return response;
                    }
                    if (attempt == retries)
                    {
                        throw new HttpRequestException($"Failed after {retries} attempts");
                    }
                    await Task.Delay(TimeSpan.FromSeconds(attempt));
                }
                return null;
            }
        }
    }
}