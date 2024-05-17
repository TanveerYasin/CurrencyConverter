namespace CurrencyConverterApi.Models
{
    public class ExchangeRatesResponse
    {
        public string Base { get; set; }
        public DateTime date { get; set; }
        public Dictionary<string, decimal> rates { get; set; }
    }
}
