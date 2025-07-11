using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Extensions;
using Api.Models;
using Api.Modles;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly AppDbContext _context;

        public PortfolioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<StockDTO>> GetUserPortfolio(AppUser appUser)
        {
            return await _context.portfolios
                .Where(p => p.AppUserId == appUser.Id)
                .Select(p => new StockDTO
                {
                    Id = p.StockId ?? 0,
                    Symbol = p.stock!.Symbol,
                    CompanyName = p.stock.CompanyName,
                    Purchase = p.stock.Purchase,
                    LastDiv = p.stock.LastDiv,
                    Industry = p.stock.Industry,
                    MarketCap = p.stock.MarketCap
                })
                .ToListAsync();
        }

        public async Task<Portfolio> CreatPortfolioAsync(Stock stock, AppUser appUser)
        {

            Portfolio portfolio = new Portfolio
            {
                StockId = stock.Id,
                AppUserId = appUser.Id
            };
            await _context.portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;
        }

        public async Task<bool> DeletePortfolioAsync(string symbol, AppUser appUser)
        {


            var stockId = await _context.Stocks
                .Where(s => EF.Functions.Like(s.Symbol, symbol))
                .Select(s => (int?)s.Id)
                .FirstOrDefaultAsync();

            if (stockId == null)
                return false;

            int deletedCount = await _context.portfolios
                .Where(p => p.AppUserId == appUser.Id && p.StockId == stockId)
                .ExecuteDeleteAsync();

            return deletedCount > 0;
        }

    }
}