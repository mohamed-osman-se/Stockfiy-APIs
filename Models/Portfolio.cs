using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Api.Modles;

namespace Api.Models
{
    [Table("portfolios")]
    public class Portfolio
    {
        public string? AppUserId { get; set; }

        public int? StockId { get; set; }

        public AppUser? appUser { get; set; }
        public Stock? stock { get; set; }

        public List<Portfolio>? portfolios { get; set; } = new List<Portfolio>();
    }
}