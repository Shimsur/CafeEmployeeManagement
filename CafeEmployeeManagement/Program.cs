using CafeEmployeeManagement.Data;
using CafeEmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 21)))
);

// Add controllers
builder.Services.AddControllers();

// Configure Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CafeEmployeeManagement API", Version = "v1" });
});

var app = builder.Build();

// Migrate and Seed the database
await MigrateDatabase(app.Services);

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CafeEmployeeManagement API v1");
        c.RoutePrefix = string.Empty;  // Access Swagger UI at root URL
    });
}

app.UseAuthorization();
app.MapControllers();
app.Run();

// Method to migrate the database and seed
async Task MigrateDatabase(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Migrate the database
    await dbContext.Database.MigrateAsync(); // This applies any pending migrations

    // Seed the database
    await SeedDatabase(dbContext);
}

// Method to seed the database
async Task SeedDatabase(ApplicationDbContext dbContext)
{
    // Seed cafes if necessary
    if (!dbContext.Cafes.Any())
    {
        var cafeSeed = new List<Cafe>
        {
            new Cafe
            {
                Id = Guid.NewGuid(),
                Name = "Cafe Mocha",
                Description = "A cozy place for coffee lovers.",
                Logo = "logo1.png",
                Location = "New York"
            },
            new Cafe
            {
                Id = Guid.NewGuid(),
                Name = "Cafe Latte",
                Description = "Best lattes in town.",
                Logo = "logo2.png",
                Location = "San Francisco"
            }
        };

        await dbContext.Cafes.AddRangeAsync(cafeSeed);
        await dbContext.SaveChangesAsync(); // Save cafes first
    }

    // Get cafes from the database again after saving them
    var cafes = await dbContext.Cafes.ToListAsync();

    // Seed employees if necessary, and only if there are enough cafes
    if (!dbContext.Employees.Any() && cafes.Count >= 2)
    {
        var employees = new List<Employee>
        {
            new Employee
            {
                Id = "UI0000001",
                Name = "John Doe",
                EmailAddress = "john.doe@example.com",
                PhoneNumber = "91234567",
                Gender = "Male",
                StartDate = DateTime.Now,
                CafeId = cafes[0].Id // Assign to first cafe
            },
            new Employee
            {
                Id = "UI0000002",
                Name = "Jane Smith",
                EmailAddress = "jane.smith@example.com",
                PhoneNumber = "81234567",
                Gender = "Female",
                StartDate = DateTime.Now,
                CafeId = cafes[1].Id // Assign to second cafe
            }
        };

        await dbContext.Employees.AddRangeAsync(employees);
        await dbContext.SaveChangesAsync(); // Save employees to the database
    }
    else if (cafes.Count < 2)
    {
        // Log an error or warning if there are not enough cafes to assign employees
        Console.WriteLine("Not enough cafes to assign employees.");
    }
}
