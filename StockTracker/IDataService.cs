namespace StockTracker
{
    public interface IDataService
    {
        public Task<List<StockPriceResponse>> GetIEXData(string symbol, DateTime startDate, DateTime endDate);
    }
}
