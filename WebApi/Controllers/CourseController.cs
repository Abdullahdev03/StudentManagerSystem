using System.Net;
using Domain.Dtos;
using Domain.Wrapper;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
/*
namespace WebApi.Controllers;
[ApiController]
[Route("[controller]")]
public class CourseController : ControllerBase
{
    private readonly CourseService _courseService;

    public CourseController(CourseService courseService)
    {
        _courseService = courseService;
    }
    
    [HttpGet("Courses")]
    [AllowAnonymous]
    public async Task<Response<List<CourseDto>>> GetCourses()
    {
        return await _courseService.GetCourse();
    }
    [HttpGet("CourseId")]
    [AllowAnonymous]
    public async Task<Response<CourseDto>> GetCourseById(int id)
    {
        return await _courseService.GetCourseById(id);
    }


    [HttpPost("AddCourse")]
    [AllowAnonymous] 
    public async Task<Response<CourseDto>> AddCourse([FromBody]CourseDto model)
    {
        if (ModelState.IsValid)
        {
            return await _courseService.AddCourse(model);
        }
        else
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage).ToList();
            return new Response<CourseDto>(HttpStatusCode.BadRequest, errors);
        }
    }

    [HttpPut("UpdateCourse")]
    [AllowAnonymous]
    public async Task<Response<CourseDto>> UpdateCourse(CourseDto model)
    {
        if (ModelState.IsValid)
        {
            return await _courseService.UpdateCourse(model);
        }
        else
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage).ToList();
            return new Response<CourseDto>(HttpStatusCode.BadRequest, errors);
        }
    }

    [HttpDelete("DeleteCourse")]
    [AllowAnonymous]
    public async Task<Response<string>> DeleteCourse([FromBody] int Id)
    {
        return await _courseService.DeleteCourse(Id);
    }
}*/