using System.Net;
using Domain.Dtos;
using Domain.Wrapper;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[ApiController]
[Route("[controller]")]
[EnableCors("MyCors")]

// [Authorize(Roles = $"{Roles.Admin}")]
public class StudentController : ControllerBase
{
    private readonly StudentService _studentService;

    public StudentController(StudentService studentService)
    {
        _studentService = studentService;
    }
    
    [HttpGet("Students")]
    public async Task<Response<List<StudentDto>>> GetStudent()
    {
        return await _studentService.GetStudent();
    }
    [HttpGet("StudentId")]
    public async Task<Response<StudentDto>> GetStudentId(int id)
    {
        return await _studentService.GetStudentById(id);
    }
    [HttpGet("StudentName")]
    public async Task<Response<StudentDto>> GetStudentName(string name)
    {
        return await _studentService.GetStudentByName(name);
    }
    [HttpGet("StudentCourse")]
    public async Task<Response<List<StudentDto>>> GetStudentCourse(int a)
    {
        return await _studentService.GetStudentByCourse(a);
    }
    [HttpGet("Studentgroup")]
    public async Task<Response<List<StudentDto>>> GetStudentgroup(string a, int b)
    {
        return await _studentService.GetStudentByGroup(a,b);
    }

    [HttpPost("AddStudent")]
    public async Task<Response<StudentDto>> AddStudent([FromBody]StudentDto model)
    {
        if (ModelState.IsValid)
        {
            return await _studentService.AddStudent(model);
        }
        else
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage).ToList();
            return new Response<StudentDto>(HttpStatusCode.BadRequest, errors);
        }
    }

    [HttpPut("UpdateStudent")]
    public async Task<Response<StudentDto>> UpdateStudent(StudentDto model)
    {
        if (ModelState.IsValid)
        {
            return await _studentService.UpdateStudent(model);
        }
        else
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage).ToList();
            return new Response<StudentDto>(HttpStatusCode.BadRequest, errors);
        }
    }

    [HttpDelete("DeleteStudent")]
    public async Task<Response<string>> DeleteStudent([FromBody] int Id)
    {
        return await _studentService.DeleteStudent(Id);
    }
}