using Domain.Constants;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.SeedData;

public class SeedData
{
    public static void Seed(DataContext context)
    {
        if (context.Roles.Any()) return;
        
        var roles = new List<IdentityRole>()
        {
            new IdentityRole(Roles.Admin){NormalizedName = Roles.Admin.ToUpper()},
            new IdentityRole(Roles.Mentor){NormalizedName = Roles.Mentor.ToUpper()},
            new IdentityRole(Roles.Student){NormalizedName = Roles.Student.ToUpper()},
            
        };
        context.Roles.AddRangeAsync(roles);
        context.SaveChangesAsync();

    }
}