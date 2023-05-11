using Domain.Dtos;
using Domain.Wrapper;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;

public interface IAccountService
{
   Task<Response<IdentityResult>> Register(RegisterDto registerDto);
    Task<Response<TokenDto>> Login(LoginDto model);
}