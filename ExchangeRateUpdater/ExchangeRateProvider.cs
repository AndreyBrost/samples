using System.Collections.Generic;
using System.Net.Http;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private const string URL = "http://api.fixer.io/latest?base={0}";

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            foreach (Currency source in currencies)
            {
                ExchangeRates rates =  GetRate(source);
                foreach (Currency target in currencies)
                {
                    if (source.Code != target.Code)
                    {
                        if (rates != null && rates.Rates != null && rates.Rates.ContainsKey(target.Code) == true)
                            yield return new ExchangeRate(source, target, rates.Rates[target.Code]);
                        else
                            continue;
                    }
                }
            }
        }

        private ExchangeRates GetRate(Currency source)
        {
            ExchangeRates res = null;
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(string.Format(URL, source.Code)).Result;
                if (response.IsSuccessStatusCode)
                {
                    res = response.Content.ReadAsAsync<ExchangeRates>().Result;
                }
            }
            return res;
        }
    }
}
