
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Net.Http.Headers;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly DataContext _context;
    private readonly ITokenService _tokenService;
    public AccountController(DataContext context, ITokenService tokenService)
    {
        _tokenService = tokenService;
        _context = context;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDTO registerdto)
    {
        //Checking if the User already exists
        if (await UserExists(registerdto.Email)) return BadRequest("This email already has an account");

        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            FirstName = registerdto.FirstName.ToLower(),
            LastName = registerdto.LastName.ToLower(),
            Email = registerdto.Email.ToLower(),
            Password = registerdto.Password,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerdto.Password)),
            PasswordSalt = hmac.Key
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new UserDto
        {
            Username = user.Email,
            Token = _tokenService.CreateToken(user)
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDTO logindto)
    {
        var user = await _context.Users.SingleOrDefaultAsync(x =>
            x.Email == logindto.Email);

        if (user == null) return Unauthorized("Invalid Username");

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var ComputedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(logindto.Password));

        for (int i = 0; i < ComputedHash.Length; i++)
        {
            if (ComputedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
        }
        Console.WriteLine("User is authenticated");
        return new UserDto
        {
            Username = user.Email,
            Token = _tokenService.CreateToken(user)
        };

    }
    private async Task<bool> UserExists(string email)
    {
        return await _context.Users.AnyAsync(x => x.Email == email.ToLower());
    }
}
