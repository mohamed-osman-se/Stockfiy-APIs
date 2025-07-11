using Api.DTOs;
using Api.Extensions;
using Api.Models;
using Api.Modles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

public class CommentController : BaseController
{
    private readonly IcommentRepository _commentRepository;
    private readonly UserManager<AppUser> _UserManager;

    public CommentController(IcommentRepository commentRepository, UserManager<AppUser> userManager)
    {
        _commentRepository = commentRepository;
        _UserManager = userManager;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,AppUser")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(ApiResponse<List<CommentDTO>>.SuccessResponse(
            await _commentRepository.GetAllAsync()));

    }

    [HttpPost]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> Creat(CreatCommentDTO creatCommentDTO)
    {

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(ApiResponse<object>.Fail("Validation Failed", errors));
        }
        var dBUser = await _UserManager.FindByEmailAsync(User.GetEmail()!);

        var comment = await _commentRepository.
        CreateAsync(creatCommentDTO, dBUser!.Id);
        return CreatedAtAction(nameof(GetById),
        new { id = comment.Id },
        ApiResponse<CommentDTO>.SuccessResponse(comment));
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var comment = await _commentRepository.GetByIdAsync(id);
        if (comment == null)
            return NotFound(ApiResponse<object>.NotFound());
        return Ok(ApiResponse<CommentDTO>.SuccessResponse(comment));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,User")]

    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UPdateCommentDTO uPdateCommentDTO)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(ApiResponse<object>.Fail("Validation Failed", errors));
        }
        var commentDto = await _commentRepository.Update(id, uPdateCommentDTO);
        if (commentDto == null)
            return NotFound(ApiResponse<object>.NotFound());
        return Ok(ApiResponse<CommentDTO>.SuccessResponse(commentDto));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteComment([FromRoute] int id)
    {
        var result = await _commentRepository.DeleteAsync(id);
        if (!result)
            return NotFound(ApiResponse<object>.NotFound());
        return NoContent();

    }

}