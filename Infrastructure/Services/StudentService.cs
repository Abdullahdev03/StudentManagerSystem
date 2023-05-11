using System.Net;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using Domain.Wrapper;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// namespace Infrastructure.Services;

public class StudentService
{
    
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public StudentService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
  // TODO: Abdulloh
      public async Task<Response<List<StudentDto>>> GetStudent()
    {
        var response = await _context.Students.Select(x => new StudentDto()
        {
            Id = x.Id,
            FirstName = x.FirstName,
            LastName = x.LastName,
            NumberPhone = x.NumberPhone,
            Birthplace = x.Birthplace,
            Locations = x.Locations,
            ParentFirstName = x.ParentFirstName,
            ParentLastName = x.ParentLastName,
            ParentPhoneNumber = x.ParentPhoneNumber,
            Course = x.Course,
            GroupName = x.Group.Name, 
            
        }).ToListAsync();
        var mapped = _mapper.Map<List<StudentDto>>(response);
        return new Response<List<StudentDto>>(mapped);
    }
    
     public async Task<Response<StudentDto>> GetStudentById(int id)
    {
        var response = await _context.Students.FindAsync(id);
        var mapped = _mapper.Map<StudentDto>(response);
        return new Response<StudentDto>(mapped);
    }
     
     public async Task<Response<StudentDto>> GetStudentByName(string name)
     {
         // filter students by name
         var students = await _context.Students.Where(s => s.FirstName.Contains(name)).ToListAsync();
         var mappedStudents = _mapper.Map<StudentDto>(students);
         return new Response<StudentDto>(mappedStudents);
     }
     public async Task<Response<List<StudentDto>>> GetStudentByCourse(int cours)
     {
         var course = _context.Students.Where(x=>x.Course==cours).Select(x=>new StudentDto()
         {
             Id = x.Id,
             FirstName = x.FirstName,
             LastName = x.LastName,
             NumberPhone = x.NumberPhone,
             Birthplace = x.Birthplace,
             Locations = x.Locations,
             ParentFirstName = x.ParentFirstName,
             ParentLastName = x.ParentLastName,
             ParentPhoneNumber = x.ParentPhoneNumber,
             Course = x.Course,
             GroupName = x.Group.Name, 
         }).ToList();
         return new Response<List<StudentDto>>(course);
     }

     public async Task<Response<List<StudentDto>>> GetStudentByGroup(string gr, int cc)
     {
         var existing = await _context.Groups.FirstOrDefaultAsync(x => x.Name.ToLower().Contains(gr.ToLower()) & x.Course == cc);
         var course = _context.Students.Where(x=>x.GroupId==existing.Id).Select(x=>new StudentDto()
         {
             Id = x.Id,
             FirstName = x.FirstName,
             LastName = x.LastName,
             NumberPhone = x.NumberPhone,
             Birthplace = x.Birthplace,
             Locations = x.Locations,
             ParentFirstName = x.ParentFirstName,
             ParentLastName = x.ParentLastName,
             ParentPhoneNumber = x.ParentPhoneNumber,
             Course = x.Course,
             GroupName = x.Group.Name, 
         }).ToList();
         return new Response<List<StudentDto>>(course);
     }

    public async Task<Response<StudentDto>> AddStudent(StudentDto student)
    {
        var existinggroup = await _context.Groups.FirstOrDefaultAsync(x => x.Name.ToLower().Contains(student.GroupName.ToLower()) & x.Course == student.Course);
        var mapped = new Student()
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Course = student.Course,
            NumberPhone = student.NumberPhone,
            Birthplace = student.Birthplace,
            Locations = student.Locations,
            ParentFirstName = student.ParentFirstName,
            ParentLastName = student.ParentLastName,
            ParentPhoneNumber = student.ParentPhoneNumber,
            GroupId = existinggroup.Id,
        };
        await _context.Students.AddAsync(mapped);
        await _context.SaveChangesAsync();

        return new Response<StudentDto>(student);
    }

    public async Task<Response<StudentDto>> UpdateStudent(StudentDto student)
    {
        var existinggroup =
            await _context.Groups.FirstOrDefaultAsync(x => x.Name.ToLower().Contains(student.GroupName.ToLower()));

        var existing = await _context.Students.FindAsync(student.Id);
        if(existing == null) return new Response<StudentDto>(HttpStatusCode.NotFound,new List<string>(){$"Not found"});
        existing.Id = student.Id;
        existing.FirstName = student.FirstName;
        existing.LastName = student.LastName;
        existing.Course = student.Course;
        existing.NumberPhone = student.NumberPhone;
        existing.Birthplace = student.Birthplace;
        existing.Locations = student.Locations;
        existing.ParentFirstName = student.ParentFirstName;
        existing.ParentLastName = student.ParentLastName;
        existing.ParentPhoneNumber = student.ParentPhoneNumber;
        existing.GroupId = existinggroup.Id;
        await _context.SaveChangesAsync();
        return new Response<StudentDto>(student);


    }


    public async Task<Response<string>> DeleteStudent(int id)
    {
        var existing = await _context.Students.FindAsync(id);
        if(existing == null) return new Response<string>(HttpStatusCode.NotFound,new List<string>(){$"Not found"});
        _context.Students.Remove(existing);
        await _context.SaveChangesAsync();
        return new Response<string>($"Deleted");
    }
}