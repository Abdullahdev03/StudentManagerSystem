using System.Net;
using Domain.Dtos;
using Domain.Wrapper;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[ApiController]
[EnableCors("MyCors")]

public class GroupController : ControllerBase
{
    private readonly GroupService _groupService;

    public GroupController(GroupService groupService)
    {
        _groupService = groupService;
    }
    
    [HttpGet("Groups")]
    [AllowAnonymous]
    public async Task<Response<List<GroupDto>>> GetGroups()
    {
        return await _groupService.GetGroups();
    }
    [HttpGet("GroupId")]
    [AllowAnonymous]
    public async Task<Response<GroupDto>> GetStudentId(int id)
    {
        return await _groupService.GetGroupById(id);
    }


    [HttpPost("AddGroup")]
    [AllowAnonymous] 
    public async Task<Response<GroupDto>> AddGroup([FromBody]GroupDto model)
    {
        if (ModelState.IsValid)
        {
            return await _groupService.AddGroup(model);
        }
        else
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage).ToList();
            return new Response<GroupDto>(HttpStatusCode.BadRequest, errors);
        }
    }

    [HttpPut("UpdateGroup")]
    [AllowAnonymous]
    public async Task<Response<GroupDto>> UpdateGroup(GroupDto model)
    {
        if (ModelState.IsValid)
        {
            return await _groupService.UpdateGroup(model);
        }
        else
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage).ToList();
            return new Response<GroupDto>(HttpStatusCode.BadRequest, errors);
        }
    }

    [HttpDelete("DeleteGroup")]
    [AllowAnonymous]
    public async Task<Response<string>> DeleteGroup([FromBody] int Id)
    {
        return await _groupService.DeleteGroup(Id);
    }
    
}