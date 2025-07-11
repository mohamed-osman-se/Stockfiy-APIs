using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DTOs
{
   public class TokenRequestDTO
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}
}