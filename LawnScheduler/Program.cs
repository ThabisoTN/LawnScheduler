using LawnScheduler.Data;
using LawnScheduler.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LawnScheduler
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            // Register ApplicationDbContext for Identity
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Register CustomDbContext for Machines table
            builder.Services.AddDbContext<CustomDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();  // For Identity

            builder.Services.AddControllersWithViews();

            // Register BookingService
            builder.Services.AddScoped<IBookingService, BookingService>();

            var app = builder.Build();

            // Seed roles, users, and machines
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    // Migrate and seed ApplicationDbContext (for Identity)
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    context.Database.Migrate();

                    await DbInitializer.SeedRoles(services);
                    await DbInitializer.SeedUsers(services);

                    // Migrate and seed CustomDbContext (for Machines)
                    var customContext = services.GetRequiredService<CustomDbContext>();
                    customContext.Database.Migrate();

                    await DbInitializer.SeedMachines(services);  // For Machines
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            await app.RunAsync();
        }
    }
}
