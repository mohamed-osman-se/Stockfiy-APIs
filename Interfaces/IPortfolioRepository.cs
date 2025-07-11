using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Api.Modles;

namespace Api.Extensions
{
    public interface IPortfolioRepository
    {
        Task<List<StockDTO>> GetUserPortfolio(AppUser appUser);
        Task<Portfolio> CreatPortfolioAsync(Stock stock, AppUser appUser);

        Task<bool> DeletePortfolioAsync(string symbol, AppUser appUser);
    }
}