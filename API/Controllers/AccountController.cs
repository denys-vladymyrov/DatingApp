using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(AppDbContext context, ITokenService tokenService): BaseApiController
{
    [HttpPost("register")] // api/account/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await EmailExists(registerDto.Email)) return  BadRequest("Email already exists");
        
        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return new UserDto
        {
            Id = user.Id,
            DisplayName = user.DisplayName,
            Email = user.Email,
            Token = tokenService.CreateToken(user)
        };
    }
    
    [HttpPost("login")] // api/account/login
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await context.Users.SingleOrDefaultAsync(x => x.Email.ToLower() == loginDto.Email.ToLower());
            
        if (user == null) return Unauthorized("Email or password is incorrect");
        
        using var hmac = new HMACSHA512(user.PasswordSalt);
        
        var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (int i = 0; i < computeHash.Length; i++)
        {
            if (computeHash[i] != user.PasswordHash[i]) return Unauthorized("Email or password is incorrect");
        }
      
        return new UserDto
        {
            Id = user.Id,
            DisplayName = user.DisplayName,
            Email = user.Email,
            Token = tokenService.CreateToken(user)
        };
    }
    

    private async Task<bool> EmailExists(string email)
    {
        return await context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());
    }
    
}