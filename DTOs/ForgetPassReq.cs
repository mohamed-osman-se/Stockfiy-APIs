using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DTOs
{
    public class ForgetPassReq
    {
        [EmailAddress]
        [Required]
        public string? Email { get; set; }
    }
}