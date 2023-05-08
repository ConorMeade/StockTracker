namespace StockTracker
{
    public class Stock
    {
        public string? TickerSymbol { get; set; }
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
        public DateTime CurrentDate { get; internal set; }
        public double DailyReturn { get; internal set; }
    }
}
