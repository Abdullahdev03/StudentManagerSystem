using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
// [Authorize(Roles = $"{Roles.Admin}, {Roles.Parent}")]
public class ApiControllerBase:ControllerBase
{
    
}