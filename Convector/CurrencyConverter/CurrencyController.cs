using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurrencyConverter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        // Static constructor to register encoding providers for specific encodings (e.g., windows-1251)
        static CurrencyController()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        /// <summary>
        /// Fetches the list of currencies from the Central Bank of Russia and returns it as JSON.
        /// </summary>
        /// <returns>List of currencies with their codes and names.</returns>
        [HttpGet("currencies")]
        public async Task<IActionResult> GetCurrencies()
        {
            try
            {
                string url = "http://www.cbr.ru/scripts/XML_daily.asp"; // URL to fetch currency data
                var responseBytes = await _httpClient.GetByteArrayAsync(url); // Fetch raw bytes from the URL
                string responseString = Encoding.GetEncoding("windows-1251").GetString(responseBytes); // Decode response using windows-1251 encoding

                // Parse the XML response
                XDocument xmlDoc = XDocument.Parse(responseString);

                // Extract currency codes and names
                var currencies = xmlDoc.Descendants("Valute")
                    .Select(valute => new CurrencyDto
                    {
                        Code = valute.Element("CharCode")?.Value ?? "UNKNOWN",
                        Name = valute.Element("Name")?.Value ?? "No name"
                    })
                    .ToList();

                // Add Russian Ruble (RUB) as the default currency
                currencies.Insert(0, new CurrencyDto { Code = "RUB", Name = "Russian Ruble" });

                return Ok(currencies);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Failed to load the list of currencies.");
            }
        }

        /// <summary>
        /// Converts a specified amount from one currency to another.
        /// </summary>
        /// <param name="fromCurrency">The source currency code (e.g., USD).</param>
        /// <param name="toCurrency">The target currency code (e.g., EUR).</param>
        /// <param name="amount">The amount to be converted.</param>
        /// <returns>The converted amount.</returns>
        [HttpGet("convert")]
        public async Task<IActionResult> ConvertCurrency(string fromCurrency, string toCurrency, decimal amount)
        {
            try
            {
                decimal fromRate = await GetExchangeRate(fromCurrency); // Get exchange rate for source currency
                decimal toRate = await GetExchangeRate(toCurrency); // Get exchange rate for target currency

                // Calculate the converted amount
                decimal result = fromCurrency == "RUB"
                    ? amount / toRate // Convert from RUB to another currency
                    : toCurrency == "RUB"
                        ? amount * fromRate // Convert to RUB from another currency
                        : (amount / fromRate) * toRate; // Convert between two foreign currencies

                return Ok(new { result });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "An error occurred during currency conversion.");
            }
        }

        /// <summary>
        /// Retrieves the exchange rate for a given currency code.
        /// </summary>
        /// <param name="currency">The currency code (e.g., USD).</param>
        /// <returns>The exchange rate for the currency.</returns>
        private async Task<decimal> GetExchangeRate(string currency)
        {
            string url = "http://www.cbr.ru/scripts/XML_daily.asp"; // URL to fetch currency data

            var responseBytes = await _httpClient.GetByteArrayAsync(url); // Fetch raw bytes from the URL
            string responseString = Encoding.GetEncoding("windows-1251").GetString(responseBytes); // Decode response using windows-1251 encoding

            // Parse the XML response
            XDocument xmlDoc = XDocument.Parse(responseString);

            if (currency == "RUB")
                return 1m; // Russian Ruble is the base currency, so its rate is 1

            // Iterate over the XML elements to find the matching currency
            foreach (var item in xmlDoc.Descendants("Valute"))
            {
                string? charCode = item.Element("CharCode")?.Value;
                if (charCode == currency)
                {
                    string? valueString = item.Element("Value")?.Value;
                    return decimal.Parse(valueString.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
                }
            }

            throw new Exception($"Exchange rate for currency {currency} not found.");
        }
    }

    /// <summary>
    /// Data transfer object for currency information.
    /// </summary>
    public class CurrencyDto
    {
        public string Code { get; set; } = string.Empty; // Currency code (e.g., USD)
        public string Name { get; set; } = string.Empty; // Currency name (e.g., US Dollar)
    }
}
