using CurrencyConverterApi.Models;
using Newtonsoft.Json.Linq;

namespace CurrencyConverterApi.ApiServices
{
    public interface ICurrencyService
    {
        Task<JObject> GetLatestRatesAsync(string baseCurrency);
        Task<JObject> ConvertAmountAsync(string fromCurrency, string toCurrency, decimal amount);
        Task<JObject> GetHistoricalRatesAsync(string baseCurrency, DateTime startDate, DateTime endDate);

    }
}