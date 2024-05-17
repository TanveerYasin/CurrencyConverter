using CurrencyConverterApi.ApiServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CurrencyConverterApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;
        private static readonly HashSet<string> ExcludedCurrencies = new HashSet<string> { "TRY", "PLN", "THB", "MXN" };
        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet("latest")]
        public async Task<string> GetLatestRates([FromQuery] string baseCurrency)
        {
            var rates = await _currencyService.GetLatestRatesAsync(baseCurrency);
            return JsonConvert.SerializeObject(rates);
        }

        [HttpGet("convert")]
        public async Task<string> ConvertCurrency([FromQuery] string from, [FromQuery] string to, [FromQuery] decimal amount)
        {
            if (ExcludedCurrencies.Contains(from) || ExcludedCurrencies.Contains(to))
            {
                return "Conversion for specified currencies is not allowed.";
            }

            var conversionResult = await _currencyService.ConvertAmountAsync(from, to, amount);
            return JsonConvert.SerializeObject(conversionResult);
        }

        [HttpGet("historical")]
        public async Task<string> GetHistoricalRates([FromQuery] string baseCurrency, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var rates = await _currencyService.GetHistoricalRatesAsync(baseCurrency, startDate, endDate);
            var pagedRates = Paginate(rates, page, pageSize);
            return JsonConvert.SerializeObject(pagedRates);
        }

        private static JObject Paginate(JObject rates, int page, int pageSize)
        {
            var pagedRates = new JObject();
            var skip = (page - 1) * pageSize;
            var take = pageSize;

            foreach (var prop in rates.Properties().Skip(skip).Take(take))
            {
                pagedRates[prop.Name] = prop.Value;
            }

            return pagedRates;
        }
    }
}