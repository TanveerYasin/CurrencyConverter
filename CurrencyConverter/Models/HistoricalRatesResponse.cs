﻿namespace CurrencyConverterApi.Models
{
    public class HistoricalRatesResponse
    {
        public string Base { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Dictionary<string, Dictionary<string, decimal>> Rates { get; set; }
    }
}
