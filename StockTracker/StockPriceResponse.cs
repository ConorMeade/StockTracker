/* 
 * 
 * Defining a schema for the response we receive from the iex endpoint:
    https://cloud.iexapis.com/stable/stock/<symbol>/chart/ 
    Sample response https://cloud.iexapis.com/stable/stock/msft/chart/1m?token={token}

    This will create a class that can properly consume the response from thie API endpoint on iexcloud.
 */

namespace StockTracker
{
    public class StockPriceResponse
    {
        public double close { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double open { get; set; }
        public string priceDate { get; set; }
        public string symbol { get; set; }
        public int volume { get; set; }
        public string id { get; set; }
        public string key { get; set; }
        public string subkey { get; set; }
        public DateTime date { get; set; }
        public long updated { get; set; }
        public double changeOverTime { get; set; }
        public double marketChangeOverTime { get; set; }
        public double uOpen { get; set; }
        public double uClose { get; set; }
        public double uHigh { get; set; }
        public double uLow { get; set; }
        public int uVolume { get; set; }
        public double fOpen { get; set; }
        public double fClose { get; set; }
        public double fHigh { get; set; }
        public double fLow { get; set; }
        public int fVolume { get; set; }
        public string label { get; set; }
        public double change { get; set; }
        public double changePercent { get; set; }
    }
}
