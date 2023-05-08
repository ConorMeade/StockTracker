using Newtonsoft.Json;

namespace StockTracker
{
    public class DataService : IDataService
    {
        static HttpClient client = new HttpClient();

        // get our tokens to use iexcloud. If tokenSecret doesn't work, try tokenPublishable
        static readonly string tokenPublishable = APIKeys.tokenPublishable;
        static readonly string tokenSecret = APIKeys.tokenSecret;

        /// <summary>
        /// Find the number of days between the two dates we pass in to /GetStockPrices.
        /// Return 0 if either of the dates are null.
        /// </summary>
        /// <param name="startDate">Beginning date that we want stock data from</param>
        /// <param name="endDate">End date that we want stock data from</param>
        /// 
        /// <returns>
        /// The number of the days between the two dates as a double.
        /// </returns>
        /// 
        private static double GetDaysBetween(DateTime startDate, DateTime endDate)
        {
            // if no dates are passed in (or if just one is missing), just use year to date data.
            if (startDate == DateTime.MinValue && endDate == DateTime.MinValue)
            {
                // return 0 here since when 0 is passed to GetDateRange(), it will return the 'ytd' slug necessary
                return 0;
            }

            double daysBetween = (endDate - startDate).TotalDays;
            if (daysBetween < 0)
            {
                return 0;
            }

            return daysBetween;
        }

        /// <summary>
        /// Given the number of days that is returned by GetDaysBetween(). We can use that to determine
        /// which date range should be used as an argument for the endpoint:
        /// 'https://cloud.iexapis.com/stable/stock/{tickerSymbol}/chart/{rangeSlug}'
        /// </summary>
        /// <param name="daysBetween">The number of days between the start and end dates</param>
        /// 
        private static string GetDateRange(double daysBetween)
        {

            string RangeSlug = "1m";

            // invalid range, use year to date changes
            if (daysBetween <= 0 || daysBetween > 365)
            {
                RangeSlug = "ytd";
            }
            else if (daysBetween > 0 && daysBetween <= 5)
            {
                RangeSlug = "5d";
            }
            // 1 month worth of stock prices
            else if (daysBetween > 5 && daysBetween <= 30)
            {
                RangeSlug = "1m";
            }
            // 3 months worth of stock prices
            else if (daysBetween > 30 && daysBetween <= 90)
            {
                RangeSlug = "3m";
            }
            // 6 month worth of stock prices
            else if (daysBetween > 90 && daysBetween <= 180)
            {
                RangeSlug = "6m";
            }
            // 1 years worth of stock prices
            else
            {
                RangeSlug = "1y";
            }

            return RangeSlug;
        }

        ///<summary>
        /// Given the stock prices in a set range, we need to selectively remove some data so we only have
        /// stock prices in the exact range specified by the startDate and endDate.
        ///</summary>
        /// <param name="stockData">Unfiltered stock data returned from iex endpoint</param>
        /// <param name="startDate">Beginning date that we want stock data from</param>
        /// <param name="endDate">End date that we want stock data from</param>
        ///
        private static List<StockPriceResponse> PruneStockData(List<StockPriceResponse> stockData, DateTime startDate, DateTime endDate) {


            List<StockPriceResponse> prunedStockData = new List<StockPriceResponse>();

            foreach (StockPriceResponse item in stockData)
            {
                if (item.date >= startDate && item.date <= endDate)
                {
                    prunedStockData.Add(item);
                }
            }
            return prunedStockData;

        }


        /// <summary>
        /// Asynchronously get stock data from iex cloud. Call separate functions for determining parameters.
        /// Consume json data and deserialize so stockData can be sent back to the StockController.cs.
        /// </summary>
        /// 
        /// <param name="tickerSymbol">Symbol used to indicate the stock we want data for</param>
        /// <param name="startDate">Beginning date that we want stock data from</param>
        /// <param name="endDate">End date that we want stock data from</param>
        /// 
        public async Task<List<StockPriceResponse>> GetIEXData(string tickerSymbol, DateTime startDate, DateTime endDate)
        {
            List<StockPriceResponse>? stockData = null;
            double daysBetween = GetDaysBetween(startDate, endDate);

            string rangeSlug = GetDateRange(daysBetween);

            string uri = $"https://cloud.iexapis.com/stable/stock/{tickerSymbol}/chart/{rangeSlug}?token={tokenSecret}";

            // async call to uri after supplying needed parameters
            var res = await client.GetAsync(uri);

            // serialize data as a string (async)
            var jsonString = await res.Content.ReadAsStringAsync();

            // deserialize data into a List of StockPriceRespons objects
            stockData = JsonConvert.DeserializeObject<List<StockPriceResponse>>(jsonString);

            // Use date bound checks to remove dates that do not fall within the given dates
            if (rangeSlug != "ytd")
            {
                // ytd will return the exact date range needed, so no need for pruning in that case
                stockData = PruneStockData(stockData, startDate, endDate);
            }

            return stockData.ToList();
        }

    }

}



