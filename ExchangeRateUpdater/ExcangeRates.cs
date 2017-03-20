using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public class ExchangeRates
    {
        [JsonProperty(PropertyName = "base")]
        public string Base{get; set;}
        [JsonProperty(PropertyName = "date")]
        public DateTime Date { get; set; }
        [JsonProperty(PropertyName = "rates")]
        public Dictionary<string, decimal> Rates { get; set; }
    }
}
