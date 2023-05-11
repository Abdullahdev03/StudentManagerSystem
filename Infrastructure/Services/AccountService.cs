using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Domain.Dtos;
using Domain.Wrapper;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public class AccountService : IAccountService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public AccountService(IConfiguration configuration, UserManager<IdentityUser> userManager, DataContext context, IMapper mapper)
    {
        _configuration = configuration;
        _userManager = userManager;
        _context = context;
        _mapper = mapper;
    }

    public async Task<Response<IdentityResult>> Register(RegisterDto registerDto)
    {
        var user = new IdentityUser()
        { 
            
            UserName = registerDto.UserName,
            PhoneNumber = registerDto.PhoneNumber,
        };
        var result = await _userManager.CreateAsync(user,registerDto.Password);
        return new Response<IdentityResult>(result);
    }

    public async Task<Response<TokenDto>> Login(LoginDto model)
    {
        var existing = await _userManager.FindByNameAsync(model.UserName);
        if (existing == null)
            return new Response<TokenDto>(HttpStatusCode.BadRequest, new List<string>(){"Incorrect password or login"});

        var check = await _userManager.CheckPasswordAsync(existing, model.Password);
        if (check == true)
        {
            return new Response<TokenDto>(await GenerateJWTToken(existing));
        }
        else 
            return new Response<TokenDto>(HttpStatusCode.BadRequest, new List<string>(){"Incorrect password or login"});

    }
    
    private async Task<TokenDto> GenerateJWTToken(IdentityUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            // new Claim(ClaimTypes.Role,"Admin")
        };

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(15),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);
        return new TokenDto(tokenString);

    }
}