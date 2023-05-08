using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Swashbuckle.Models;

/// <summary>
/// Stock data
/// </summary>
public class Stock
{

    /// <summary>
    /// Stock ticker symbol (i.e. msft, aapl, tsla)
    /// </summary>
    [Required]
    public string? TickerSymbol { get; set; }
    /// <summary>
    /// Low end date for price data
    /// </summary>
    public DateTime FromDate { get; set; }

    /// <summary>
    /// High end date for price data
    /// </summary>
    public DateTime ToDate { get; set; }

    /// <summary>
    /// Current date of stock data
    /// </summary>
    public DateTime CurrentDate { get; set; }

    /// <summary>
    /// Difference of price at close and price at open
    /// </summary>
    public double DailyReturn { get; set; }
}
