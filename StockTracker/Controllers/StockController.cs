using Microsoft.AspNetCore.Mvc;

namespace StockTracker.Controllers
{
    [ApiController]
    [Route("GetStockPrices")]
    [Produces("application/json")]
    public class StockController : ControllerBase
    {
        // Init logger to see the flow of the program, (DI?)
        private readonly ILogger<StockController> _logger;
        private readonly IDataService _dataService;

        public StockController(ILogger<StockController> logger, IDataService ds)
        {
            _logger = logger;
            _dataService = ds;
        }

        /// <summary>
        /// Asynchronously take results from iex endpoint and format them as new objects
        /// that follow the schema of the Stock class.
        /// <param name="tickerSymbol">Symbol used to indicate the stock we want data for</param>
        /// <param name="startDate">Beginning date that we want stock data from</param>
        /// <param name="endDate">End date that we want stock data from</param>
        /// 
        /// <returns>
        /// A list of Stock objects. Includes the symbol, date range, current date, 
        /// and the daily returns for that day.
        /// </returns>
        /// </summary>
        ///
        protected async Task<Stock[]> FormatTaskResults(string tickerSymbol, DateTime startDate, DateTime endDate)
        {
            // DataService ds = new DataService();
            // If no startDate passed, use the date from beginning of year to endDate
            if (startDate == DateTime.MinValue)
            {
                int year = DateTime.Now.Year;
                startDate = new DateTime(year, 1, 1);
            }

            // Use todays date if endDate is missing
            if (endDate == DateTime.MinValue)
            {
                endDate = DateTime.Now;
            }

            _logger.LogInformation("call to FormatTaskResults()");
            // will be of type Task<List<StockPriceResponse>>, use separate DataService to handle getting external data
            List<StockPriceResponse> results = new List<StockPriceResponse>();
            try
            {
                results = await _dataService.GetIEXData(tickerSymbol, startDate, endDate);
            } catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            var lstStockObjects = new List<Stock>();

            _logger.LogInformation("Successfully fetched data from iexcloud!");
            foreach (var result in results)
            {
                lstStockObjects.Add(new Stock
                {
                    TickerSymbol = result.symbol,
                    FromDate = startDate,
                    ToDate = endDate,
                    CurrentDate = result.date,
                    DailyReturn = result.open - result.close,
                });
            }

            return lstStockObjects.ToArray();
        }

        /// <summary>
        /// Controller that takes in our parameters, passes them along and then asynchronously returns
        /// our Stock objects with data about those days. Parameter error handling is also done here.
        /// *** Currently can only return data within the last year
        /// </summary>
        /// <param name="tickerSymbol">Symbol used to indicate the stock we want data for</param>
        /// <param name="startDate">Beginning date that we want stock data from</param>
        /// <param name="endDate">End date that we want stock data from</param>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /GetStockPrices
        ///     
        ///     {
        ///         "tickerSymbol": "MSFT",
        ///         "fromDate": "2023-04-10T00:00:00",
        ///         "toDate": "2023-04-20T00:00:00",
        ///         "currentDate": "2023-04-11T00:00:00",
        ///         "dailyReturn": 2.920000000000016
        ///     }
        ///       
        /// </remarks>
        /// 
        /// <returns>
        /// A list of Stock objects. Includes the symbol, date range, current date, 
        /// and the daily returns for that day.
        /// </returns>
        /// 
        [HttpGet(Name = "GetStockPrices")]
        public async Task<Stock[]> GetStockPrices(string tickerSymbol, DateTime startDate, DateTime endDate)
        {
            _logger.LogInformation("call to GetStockPrices()", tickerSymbol, startDate, endDate);
            // handle cases where the time span is greater than 1 year
            // check min value since DateTime can't be null, it will automatically be assigned min val if missing from query
            if (startDate != DateTime.MinValue && endDate != DateTime.MinValue)
            {
                double daysBetween = (endDate - startDate).TotalDays;

                if (daysBetween > 365 || daysBetween < 0)
                {
                    throw new Exception("The provided dates range is not within the bounds of 1 day - 1 year (365 days)");
                }
            }

            // missing tickerSymbol arg
            if (tickerSymbol == null)
            {
                throw new Exception("No stock ticker symbol was passed, unable to fetch stock data.");
            }

            // ticker symbol length can be at max 5 chars
            if (tickerSymbol.Length > 5) {
                throw new Exception("Ticker symbols cannot be longer than 5 characters");
            
            }

            var StockData = FormatTaskResults(tickerSymbol, startDate, endDate);

            return await StockData;
        }

    }
}
