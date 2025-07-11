using System.Net.Http.Headers;
using System.Net.Quic;
using Api.DTOs;
using Api.Modles;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

public class StockRepository : IstockRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public StockRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<StockDTO>> GetAllAsync()
    {
        return await _context.Stocks
       .AsNoTracking()
       .ProjectTo<StockDTO>(_mapper.ConfigurationProvider)
       .ToListAsync();
    }

    public async Task<StockDTO?> GetByIdAsync(int id)
    {
        return await _context.Stocks
            .AsNoTracking()
            .Where(s => s.Id == id)
            .ProjectTo<StockDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }

    public async Task<StockDTO> CreateAsync(CreatStockDTO stockPostDTO)
    {
        var stock = _mapper.Map<Stock>(stockPostDTO);
        _context.Stocks.Add(stock);
        await _context.SaveChangesAsync();
        return _mapper.Map<StockDTO>(stock);
    }

    public async Task<StockDTO?> UpdateAsync(int id, UpdateStockDTO updateStockDTO)
    {
        var stock = await _context.Stocks.FindAsync(id);
        if (stock == null) return null;

        _mapper.Map(updateStockDTO, stock);
        await _context.SaveChangesAsync();
        return _mapper.Map<StockDTO>(stock);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var stock = await _context.Stocks.FindAsync(id);
        if (stock == null) return false;

        _context.Stocks.Remove(stock);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }


    public async Task<List<StockDTO>> Filter(QueryFilter queryFilter)
    {
        var query = _context.Stocks
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(queryFilter.CompanyName))
        {
            query = query.Where(s => s.CompanyName == queryFilter.CompanyName);
        }

        if (!string.IsNullOrWhiteSpace(queryFilter.Symbol))
        {
            query = query.Where(s => s.Symbol == queryFilter.Symbol);
        }

        if (!string.IsNullOrWhiteSpace(queryFilter.SortBy) &&
        queryFilter.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
        {
            query = queryFilter.IsDecsending
            ? query.OrderByDescending(s => s.Symbol)
             : query.OrderBy(s => s.Symbol);
        }
        query = query.Skip((queryFilter.PageNum - 1) * queryFilter.pageSize)
        .Take(queryFilter.pageSize);
        return await query
            .ProjectTo<StockDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }


    public async Task<Stock> GetBySymbolAsync(string symbol)
    {
        var stock = await _context.Stocks
      .FirstOrDefaultAsync(s => EF.Functions.Like(s.Symbol, symbol));
        return stock!;

    }

}
