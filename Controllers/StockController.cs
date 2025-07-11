
using Api.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

public class StockController : BaseController
{
    private readonly IstockRepository _stockRepo;

    public StockController(IstockRepository stockRepo)
    {
        _stockRepo = stockRepo;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,User")]

    public async Task<IActionResult> GetAll()
    {
        var stocks = await _stockRepo.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<StockDTO>>.SuccessResponse(stocks));
    }

    [HttpGet("filter")]
    [Authorize(Roles = "Admin,User")]

    public async Task<IActionResult> Filter([FromQuery] QueryFilter queryFilter)
    {
        var stocks = await _stockRepo.Filter(queryFilter);
        return Ok(ApiResponse<IEnumerable<StockDTO>>.SuccessResponse(stocks));
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,User")]

    public async Task<IActionResult> GetById(int id)
    {
        var stock = await _stockRepo.GetByIdAsync(id);
        if (stock == null)
            return NotFound(ApiResponse<object>.NotFound());

        return Ok(ApiResponse<StockDTO>.SuccessResponse(stock));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreatStockDTO dto)
    {

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(ApiResponse<object>.Fail("Validation Failed", errors));
        }

        var stockDto = await _stockRepo.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = stockDto.Id },
            ApiResponse<StockDTO>.SuccessResponse(stockDto));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateStockDTO dto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(ApiResponse<object>.Fail("Validation Failed", errors));
        }

        var updatedStock = await _stockRepo.UpdateAsync(id, dto);
        if (updatedStock == null)
            return NotFound(ApiResponse<object>.NotFound());

        return Ok(ApiResponse<StockDTO>.SuccessResponse(updatedStock));
    }
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _stockRepo.DeleteAsync(id);
        if (!result)
            return NotFound(ApiResponse<object>.NotFound());

        return NoContent();
    }
}





