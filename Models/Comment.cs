using System.ComponentModel.DataAnnotations.Schema;
using Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Modles;

[Table("Comments")]
public class Comment
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string content { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public int? StockId { get; set; }
    public Stock? stock { get; set; }
    public string? AppUserId { get; set; }
    public AppUser? appUser { get; set; }
}







