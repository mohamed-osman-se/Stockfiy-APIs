using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.Config;
using Api.DTOs;
using Api.Interfaces;
using Api.Models;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;

namespace Api.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        private readonly IEmailService _emailService;

        private readonly AppDbContext _context;

        public AccountController(AppDbContext context, IEmailService emailService, ITokenService tokenService, UserManager<AppUser> userManager, IOptions<JwtSettings> jwtSettings, IMapper mapper)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _mapper = mapper;
            _tokenService = tokenService;
            _emailService = emailService;
            _context = context;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> register([FromBody] RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(ApiResponse<object>.Fail("Validation Failed", errors));
            }
            try
            {
                var appUser = _mapper.Map<AppUser>(registerDTO);
                var createdUser = await _userManager.CreateAsync(appUser, registerDTO.Password!);
                if (createdUser.Succeeded)
                {
                    var userRole = await _userManager.AddToRoleAsync(appUser, "User");
                    if (userRole.Succeeded)
                    {
                        var authResponse = _mapper.Map<AuthResponse>(appUser);
                        var roles = await _userManager.GetRolesAsync(appUser);
                        authResponse.AccessToken = _tokenService.GenerateJWT(appUser, roles);
                        authResponse.RefreshToken = _tokenService.GenerateRefreshToken();
                        appUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);
                        appUser.RefreshToken = authResponse.RefreshToken;
                        await _userManager.UpdateAsync(appUser);
                        return Ok(ApiResponse<object>.SuccessResponse(authResponse, "User created successfully!"));
                    }
                    else
                        return BadRequest(ApiResponse<object>.
                        Fail("Registeration Failed", userRole.Errors.Select(e => e.Description).ToList()));
                }
                else
                    return BadRequest(ApiResponse<object>.
                    Fail("Registeration Failed", createdUser.Errors.Select(e => e.Description).ToList()));

            }
            catch (System.Exception ex)
            {

                return BadRequest(ApiResponse<object>.Fail(
                    "Registration Failed",
                    new List<string> { ex.Message }));

            }

        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(ApiResponse<object>.Fail("Validation Failed", errors));
            }

            var dBuser = await _userManager.FindByEmailAsync(loginDTO.Email!);
            if (dBuser == null ||
             !(await _userManager.CheckPasswordAsync(dBuser, loginDTO.Password!)))
            {
                return Unauthorized(ApiResponse<object>.Unauthorized("Invalid Credentials "));
            }
            var authResponse = _mapper.Map<AuthResponse>(dBuser);
            var roles = await _userManager.GetRolesAsync(dBuser);
            authResponse.AccessToken = _tokenService.GenerateJWT(dBuser, roles);
            authResponse.RefreshToken = _tokenService.GenerateRefreshToken();
            dBuser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);
            dBuser.RefreshToken = authResponse.RefreshToken;
            await _userManager.UpdateAsync(dBuser);
            return Ok(ApiResponse<object>.SuccessResponse(authResponse, "User logged successfully!"));
        }



        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh(TokenRequestDTO tokenRequest)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(tokenRequest.AccessToken);
            if (principal == null) return BadRequest("Invalid access token");

            var dBuser = await _userManager.FindByEmailAsync(principal.FindFirst(ClaimTypes.Email)!.Value);
            if (dBuser == null ||
                dBuser.RefreshToken != tokenRequest.RefreshToken ||
                dBuser.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return Unauthorized("Invalid refresh token");
            }
            var authResponse = _mapper.Map<AuthResponse>(dBuser);
            var roles = await _userManager.GetRolesAsync(dBuser);
            authResponse.AccessToken = _tokenService.GenerateJWT(dBuser, roles);
            authResponse.RefreshToken = _tokenService.GenerateRefreshToken();
            dBuser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);
            dBuser.RefreshToken = authResponse.RefreshToken;
            await _userManager.UpdateAsync(dBuser);
            return Ok(ApiResponse<object>.SuccessResponse(authResponse, "User refreshed successfully!"));

        }

        [HttpPost("Forgot-Password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgetPassReq forgetPassReq)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(ApiResponse<object>.Fail("Validation Failed", errors));
            }
            var dBuser = await _userManager.FindByEmailAsync(forgetPassReq.Email!);
            if (dBuser == null)
            {
                return BadRequest(ApiResponse<object>.Fail());
            }
            var code = new Random().Next(100000, 999999).ToString();
            var resetCode = new PasswordResetCode
            {
                Email = forgetPassReq.Email!,
                Code = code,
                ExpiryTime = DateTime.UtcNow.AddMinutes(10)
            };

            _context.passwordResetCodes.Add(resetCode);
            await _context.SaveChangesAsync();
            var emailSent = await _emailService.SendAsync(
                resetCode.Email,
                "Stockify - Password Reset Code",
                $@"
<!DOCTYPE html>
<html>
<head>
  <meta charset=""UTF-8"">
  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
  <style>
    body {{
      font-family: Arial, sans-serif;
      background-color: #f4f4f4;
      color: #333;
      padding: 20px;
    }}
    .container {{
      background-color: #fff;
      border-radius: 8px;
      padding: 30px;
      max-width: 600px;
      margin: auto;
      box-shadow: 0 2px 8px rgba(0,0,0,0.1);
    }}
    .header {{
      text-align: center;
      margin-bottom: 30px;
    }}
    .code {{
      font-size: 36px;
      font-weight: bold;
      color: #4CAF50;
      letter-spacing: 5px;
      text-align: center;
      margin: 30px 0;
    }}
    .footer {{
      font-size: 12px;
      color: #999;
      text-align: center;
      margin-top: 40px;
    }}
  </style>
</head>
<body>
  <div class=""container"">
    <div class=""header"">
      <h2> Password Reset Request</h2>
      <p>You're receiving this email because you requested to reset your password on <strong>Stockify</strong>.</p>
    </div>

    <p>Please use the code below to reset your password. This code is valid for <strong>10 minutes</strong>.</p>

    <div class=""code"">{code}</div>

    <p>If you didn't request a password reset, you can safely ignore this email.</p>

    <div class=""footer"">
      &copy; {DateTime.UtcNow.Year} Stockify Inc. All rights reserved.
    </div>
  </div>
</body>
</html>
");


            if (!emailSent)
            {


                return BadRequest(ApiResponse<object>.Fail("Try again later!"));
            }
            return Ok(ApiResponse<object>.SuccessMessage("Check your email for the reset code."));
        }


        //ResetPassCodeDTO

        [HttpPost("Reset-Password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassCodeDTO resetpass)
        {


            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(ApiResponse<object>.Fail("Validation Failed", errors));
            }



            var codeEntry = await _context.passwordResetCodes
            .FirstOrDefaultAsync(x =>
            x.Email == resetpass.Email &&
            x.Code == resetpass.Code &&
             x.ExpiryTime > DateTime.UtcNow);

            if (codeEntry == null)
                return BadRequest(ApiResponse<object>.Fail());

            var user = await _userManager.FindByEmailAsync(resetpass.Email);
            if (user == null)
                return BadRequest("User not found.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, resetpass.NewPassword);
            _context.passwordResetCodes.Remove(codeEntry);
            await _context.SaveChangesAsync();
            if (result.Succeeded)
                return Ok(ApiResponse<object>.SuccessMessage("Password reset successfully."));

            return BadRequest(ApiResponse<object>.Fail("Try agin later",
      result.Errors.Select(e => e.Description).ToList()
  ));
        }
    }
}
