using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using LawnScheduler.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

public class DbInitializer
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        await SeedRoles(serviceProvider);
        await SeedUsers(serviceProvider);
        await SeedMachines(serviceProvider);
    }

    // Seed Roles Method
    public static async Task SeedRoles(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roleNames = { "Conflict Manager", "Machine Operator", "Customer" };

        foreach (var roleName in roleNames)
        {
           
            if (!await roleManager.RoleExistsAsync(roleName))
            {
               
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    //Seed users method

    public static async Task SeedUsers(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();


        var users = new[]
        {
         // Conflict Manager
         new { Email = "conflictmanager@gmail.com", Role = "Conflict Manager", Password = "@Thabiso111"},

         // Machine Operators
         new { Email = "operator1@gmail.com", Role = "Machine Operator", Password = "@Thabiso111" },
         new { Email = "operator2@gmail.com", Role = "Machine Operator", Password = "@Thabiso111" },
         new { Email = "operator3@gmail.com", Role = "Machine Operator", Password = "@Thabiso111" },
         new { Email = "operator4@gmail.com", Role = "Machine Operator", Password = "@Thabiso111" },
         new { Email = "operator5@gmail.com", Role = "Machine Operator", Password = "@Thabiso111" },
         new { Email = "operator6@gmail.com", Role = "Machine Operator", Password = "@Thabiso111" },

         // Customers
         new { Email = "customer1@gmail.com", Role = "Customer", Password = "@Thabiso111" },
         new { Email = "customer2@gmail.com", Role = "Customer", Password = "@Thabiso111" }
     };

        foreach (var userInfo in users)
        {
            var user = await userManager.FindByEmailAsync(userInfo.Email);
            if (user == null)
            {

                user = new IdentityUser { UserName = userInfo.Email, Email = userInfo.Email };
                var result = await userManager.CreateAsync(user, userInfo.Password);

                if (result.Succeeded)
                {

                    user.EmailConfirmed = true;
                    await userManager.UpdateAsync(user);

                    if (await roleManager.RoleExistsAsync(userInfo.Role))
                    {
                        await userManager.AddToRoleAsync(user, userInfo.Role);
                    }
                }
            }
        }
    }



    //Seeding machines method

    public static async Task SeedMachines(IServiceProvider serviceProvider)
    {
        var customContext = serviceProvider.GetRequiredService<CustomDbContext>(); 
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        if (customContext.Machines.Any())  
        {
            return;  
        }

        var operators = new[]
        {
        await userManager.FindByEmailAsync("operator1@gmail.com"),
        await userManager.FindByEmailAsync("operator2@gmail.com"),
        await userManager.FindByEmailAsync("operator3@gmail.com"),
        await userManager.FindByEmailAsync("operator4@gmail.com"),
        await userManager.FindByEmailAsync("operator5@gmail.com"),
        await userManager.FindByEmailAsync("operator6@gmail.com")
    };

        var machines = new[]
        {
        new Machine { MachineName = "Grass Cutter", Model = "Husqvarna 122HD60", Description = "Gas-powered grass cutter", OperatorId = operators[0].Id },
        new Machine { MachineName = "Ride-on Mower", Model = "John Deere X350", Description = "Ride-on mower for large lawns", OperatorId = operators[1].Id },
        new Machine { MachineName = "Hedge Trimmer", Model = "Stihl HS 45", Description = "Lightweight hedge trimmer", OperatorId = operators[2].Id },
        new Machine { MachineName = "Leaf Blower", Model = "Echo PB-580T", Description = "Backpack leaf blower", OperatorId = operators[3].Id },
        new Machine { MachineName = "Weed Wacker", Model = "DeWalt DCST920P1", Description = "Electric weed wacker", OperatorId = operators[4].Id },
        new Machine { MachineName = "Edger", Model = "BLACK+DECKER LST400", Description = "Cordless electric edger", OperatorId = operators[5].Id }
    };

        customContext.Machines.AddRange(machines);  // Add machines to CustomDbContext
        await customContext.SaveChangesAsync();
    }

}
