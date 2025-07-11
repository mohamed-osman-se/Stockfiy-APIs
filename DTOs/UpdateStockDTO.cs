using System.ComponentModel.DataAnnotations;

public class UpdateStockDTO
{
    [Range(0.01, double.MaxValue, ErrorMessage = "Purchase must be a positive number")]
    public decimal Purchase { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "LastDiv cannot be negative")]
    public decimal LastDiv { get; set; }

    [Range(0, long.MaxValue, ErrorMessage = "MarketCap cannot be negative")]
    public long MarketCap { get; set; }
}
