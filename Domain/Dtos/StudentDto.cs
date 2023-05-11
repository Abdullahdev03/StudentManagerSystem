using Domain.Entities;

namespace Domain.Dtos;

public class StudentDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Course { get; set; }
    public string GroupName { get; set; }
    public string NumberPhone { get; set; }
    public string Birthplace { get; set; }
    public string Locations { get; set; }

    public string ParentFirstName { get; set; }
    public string ParentLastName { get; set; }
    public string ParentPhoneNumber { get; set; }
    

    public StudentDto()
    {
        
    }
}

