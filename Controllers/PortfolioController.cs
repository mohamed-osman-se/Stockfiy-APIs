using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Extensions;
using Api.Models;
using Api.Modles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize(Roles = "Admin,User")]

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PortfolioController : BaseController
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly IstockRepository _stockRepo;
        private readonly IPortfolioRepository _portfolioRepo;

        public PortfolioController(UserManager<AppUser> userManager, IstockRepository stockRepo, IPortfolioRepository portfolioRepo)
        {
            _userManager = userManager;
            _stockRepo = stockRepo;
            _portfolioRepo = portfolioRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var Email = User.GetEmail();
            var dBuser = await _userManager.FindByEmailAsync(Email!);
            return Ok(ApiResponse<IEnumerable<StockDTO>>.
            SuccessResponse(await _portfolioRepo.GetUserPortfolio(dBuser!)));
        }

        [HttpPost("{symbol}")]
        public async Task<IActionResult> CreatPortfolio([FromRoute] string symbol)
        {
            var dBuser = await _userManager.FindByEmailAsync(User.GetEmail()!);
            var stock = await _stockRepo.GetBySymbolAsync(symbol);
            if (stock == null)
                return NotFound(ApiResponse<object>.NotFound());
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(dBuser!);
            if (userPortfolio.Any(p => p.Symbol.ToLower() == symbol.ToLower()))
                return BadRequest(ApiResponse<object>.Fail("Cannot add same stock to portfolio!"));
            Portfolio portfolio = await _portfolioRepo.CreatPortfolioAsync(stock, dBuser!);

            return Ok(ApiResponse<object>.SuccessMessage("Stock added successsfully!"));

        }

        [HttpDelete("{symbol}")]
        public async Task<IActionResult> DeletePortfolio([FromRoute] string symbol)
        {
            var dBuser = await _userManager.FindByEmailAsync(User.GetEmail()!);
            if (!(await _portfolioRepo.DeletePortfolioAsync(symbol, dBuser!)))
                return BadRequest(ApiResponse<object>.Fail("Stock notfound in your portfolio!"));
            return Ok(ApiResponse<object>.SuccessMessage("Stock deleted successfully!"));
        }
    }
}