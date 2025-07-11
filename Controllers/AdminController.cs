using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.DTOs;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        UserManager<AppUser> _userManager;
        AppDbContext _context;

        public AdminController(UserManager<AppUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete-User")]
        public async Task<IActionResult> DeleteUserByEmail([FromBody] ForgetPassReq email)
        {
            var user = await _userManager.FindByEmailAsync(email.Email!);

            if (user == null)
                return NotFound(ApiResponse<object>.NotFound());

            var portfolios = await _context.portfolios
                .Where(p => p.AppUserId == user.Id)
                .ToListAsync();
            var comments = await _context.Comments
                .Where(c => c.AppUserId == user.Id)
                .ToListAsync();
            _context.portfolios.RemoveRange(portfolios);
            _context.Comments.RemoveRange(comments);
            await _context.SaveChangesAsync();
            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
                return BadRequest(ApiResponse<object>.Fail("Somthing Wrong try agin later",
                result.Errors.Select(e => e.Description).ToList()));

            return Ok(ApiResponse<object>.SuccessMessage("User deleted successfully!"));
        }
    }
}