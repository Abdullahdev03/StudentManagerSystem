using System.ComponentModel.DataAnnotations;
using Domain.Dtos;

namespace Domain.Entities;

public class Group
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Course { get; set; }
    public List<Student> Students { get; set; }
    

    public Group()
    {
        
    }

    public Group(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}