using Domain.Dtos;

namespace Domain.Entities;

public class Student
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Course { get; set; }
    public string NumberPhone { get; set; }
    public string Birthplace { get; set; }
    public string Locations { get; set; }
    public string ParentFirstName { get; set; }
    public string ParentLastName { get; set; }
    public string ParentPhoneNumber { get; set; }
    public int GroupId { get; set; }
    public Group Group { get; set; }
    
    public Student()
    {
        
    }

    public Student(int id, string firstName, string lastName,int course, string numberPhone, 
        string birthplace, string locations, string parentFirstName, string parentLastName, 
        string parentPhoneNumber )
    {
        // Id = Guid.NewGuid().ToString();
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Course = course;
        NumberPhone = numberPhone;
        Birthplace = birthplace;
        Locations = locations;
        ParentFirstName = parentFirstName;
        ParentLastName = parentLastName;
        ParentPhoneNumber = parentPhoneNumber;
    }
}