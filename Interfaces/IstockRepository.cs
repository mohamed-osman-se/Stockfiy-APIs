using Api.DTOs;
using Api.Modles;

public interface IstockRepository
{
    Task<List<StockDTO>> GetAllAsync();
    Task<StockDTO?> GetByIdAsync(int id);
    Task<Stock> GetBySymbolAsync(string symbol);
    Task<StockDTO> CreateAsync(CreatStockDTO stockPostDTO);
    Task<StockDTO?> UpdateAsync(int id, UpdateStockDTO updateStockDTO);
    Task<bool> DeleteAsync(int id);
    Task<List<StockDTO>> Filter(QueryFilter queryFilter);
    Task SaveChangesAsync();
} 
