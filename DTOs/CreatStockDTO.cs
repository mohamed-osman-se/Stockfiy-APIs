using System.ComponentModel.DataAnnotations;

public class CreatStockDTO
{
    [Required(ErrorMessage = "Symbol is required")]
    [MinLength(1)]
    [MaxLength(10, ErrorMessage = "Symbol cannot exceed 10 characters")]
    public string Symbol { get; set; } = string.Empty;

    [Required(ErrorMessage = "Company Name is required")]
    [MinLength(3, ErrorMessage = "Company name must be at least 3 characters")]
    [MaxLength(100, ErrorMessage = "Company name cannot exceed 100 characters")]
    public string CompanyName { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "Purchase must be a positive number")]
    public decimal Purchase { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "LastDiv cannot be negative")]
    public decimal LastDiv { get; set; }

    [Required(ErrorMessage = "Industry is required")]
    [MinLength(3, ErrorMessage = "Industry must be at least 3 characters")]
    [MaxLength(50, ErrorMessage = "Industry cannot exceed 50 characters")]
    public string Industry { get; set; } = string.Empty;

    [Range(0, long.MaxValue, ErrorMessage = "MarketCap cannot be negative")]
    public long MarketCap { get; set; }
}
