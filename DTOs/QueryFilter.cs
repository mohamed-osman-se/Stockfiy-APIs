using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DTOs
{
    public class QueryFilter
    {
        [MinLength(3, ErrorMessage = "Title must be at least 5 characters")]
        [MaxLength(50, ErrorMessage = "Title cannot exceed 100 characters")]
        public string? Symbol { get; set; } = null;
        public string? CompanyName { get; set; } = null;
        public string? SortBy { get; set; } = null;
        public bool IsDecsending { get; set; } = false;
        public int PageNum { get; set; } = 1;
        public int pageSize { get; set; } = 5;
    }
}