using System.Net;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using Domain.Wrapper;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class GroupService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GroupService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    
    public async Task<Response<List<GroupDto>>> GetGroups()
    {
        var response = await _context.Groups.Select(x => new GroupDto()
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Course = x.Course,
            
        }).ToListAsync();
        var mapped = _mapper.Map<List<GroupDto>>(response);
        return new Response<List<GroupDto>>(mapped);
    }
    
    public async Task<Response<GroupDto>> GetGroupById(int id)
    {
        var response = await _context.Groups.FindAsync(id);
        var mapped = _mapper.Map<GroupDto>(response);
        return new Response<GroupDto>(mapped);
    }

    public async Task<Response<GroupDto>> AddGroup(GroupDto group)
    {
        var mapped = _mapper.Map<Group>(group);
        await _context.Groups.AddAsync(mapped);
        await _context.SaveChangesAsync();

        return new Response<GroupDto>(_mapper.Map<GroupDto>(mapped));
    }

    public async Task<Response<GroupDto>> UpdateGroup(GroupDto group)
    {
        var existing = await _context.Groups.FindAsync(group.Id);
        if(existing == null) return new Response<GroupDto>(HttpStatusCode.NotFound,new List<string>(){$"Not found"});
        existing.Id = group.Id;
        existing.Name = group.Name;
        existing.Description = group.Name;
        existing.Course = group.Course;
        await _context.SaveChangesAsync();
        return new Response<GroupDto>(group);


    }


    public async Task<Response<string>> DeleteGroup(int id)
    {
        var existing = await _context.Groups.FindAsync(id);
        if(existing == null) return new Response<string>(HttpStatusCode.NotFound,new List<string>(){$"Not found"});
        _context.Groups.Remove(existing);
        await _context.SaveChangesAsync();
        return new Response<string>($"Deleted");
    }
}