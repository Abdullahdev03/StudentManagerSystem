using Domain.Dtos;
using Domain.Wrapper;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[ApiController]
[Route("[controller]")]
[EnableCors("MyCors")]
// [Authorize(Roles = $"{Roles.Admin}")]
public class AccontController
{
    private readonly IAccountService _accountService;

    public AccontController(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    [HttpPost("Register")]
    [AllowAnonymous]
    public async Task<Response<IdentityResult>> Register([FromBody] RegisterDto model)
    {
        return await _accountService.Register(model);
    }

    [HttpPost("LogIn")]
    public async Task<Response<TokenDto>> LogIn([FromBody] LoginDto login)
    {
        return await _accountService.Login(login);
    }
}