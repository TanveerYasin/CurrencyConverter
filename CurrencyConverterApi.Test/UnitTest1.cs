using CurrencyConverterApi.ApiServices;
using CurrencyConverterApi.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CurrencyConverterApi.Test
{
    [TestClass]
    public class UnitTest1
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly CurrencyService _currencyService;
        public UnitTest1()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object) { BaseAddress = new Uri("https://api.frankfurter.app/") };
            _currencyService = new CurrencyService(_httpClient);
            //_httpClient = new HttpClient() { BaseAddress = new Uri("https://api.frankfurter.app/") };
            //_currencyService = new CurrencyService(_httpClient);
        }

        //Method to get exchange rates
        [TestMethod]
        public async Task GetLatestRatesAsync_ReturnsExchangeRates()
        {
            // Arrange Accordingly This is Just for EUR to USD
            var expectedResponse = new ExchangeRatesResponse { Base = "EUR", date = DateTime.Today, rates = new Dictionary<string, decimal> { { "USD", 1.2M } } };
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedResponse), Encoding.UTF8, "application/json")
            };
            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(responseMessage);

            // Act
            var result = await _currencyService.GetLatestRatesAsync("EUR");

        }

        //Method for getting unsupported currency validation
        [TestMethod]
        public async Task ConvertAmountAsync_UnsupportedCurrency_ThrowsInvalidOperationException()
        {
            // Arrange it Accordingly
            var fromCurrency = "TRY";
            var toCurrency = "USD";
            var amount = 100m;

            // Act & Assert
            object value = await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => _currencyService.ConvertAmountAsync(fromCurrency, toCurrency, amount));
        }


        [TestMethod]
        public async Task ConvertAmountAsync_SupportedCurrencies_ReturnsConvertedAmount()
        {
            // Arrange it Accordingly
            var fromCurrency = "EUR";
            var toCurrency = "USD";
            var amount = 100m;
            var expectedResponse = new ExchangeRatesResponse
            {
                Base = fromCurrency,
                date = DateTime.Today,
                rates = new Dictionary<string, decimal> { { toCurrency, 1.2m } }
            };
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedResponse), Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _currencyService.ConvertAmountAsync(fromCurrency, toCurrency, amount);

        }
        [TestMethod]
        public async Task GetHistoricalRatesAsync()
        {
            // Arrange Accordingly 
            var baseCurrency = "EUR";
            var startDate = new DateTime(2024, 05, 01);
            var endDate = new DateTime(2024, 05, 20);
            var historicalData = new Dictionary<string, Dictionary<string, decimal>>
            {
                { "2024-05-01", new Dictionary<string, decimal> { { "USD", 1.1m } } },
                { "2024-05-02", new Dictionary<string, decimal> { { "USD", 1.1m } } },
                { "2024-05-03", new Dictionary<string, decimal> { { "USD", 1.1m } } },
                { "2024-05-04", new Dictionary<string, decimal> { { "USD", 1.1m } } },
                { "2024-05-24", new Dictionary<string, decimal> { { "USD", 1.1m } } },
                { "2023-05-25", new Dictionary<string, decimal> { { "USD", 1.2m } } }
            };
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(historicalData), Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _currencyService.GetHistoricalRatesAsync(baseCurrency, startDate, endDate);

        }
    }
}