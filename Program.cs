using ContactApp.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);


// Configuration of SQLite database context for Entity Framework Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
           .UseLazyLoadingProxies()); // Enables lazy loading for navigation properties

// Add controllers to the service collection for handling API requests
builder.Services.AddControllers();

// Configuration of Swagger
builder.Services.AddEndpointsApiExplorer(); // Enables API explorer for Swagger
builder.Services.AddSwaggerGen(); // Adds Swagger generation services

// Configuration of Cross-Origin Resource Sharing (CORS) policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy => // Defines a CORS policy 
        policy.AllowAnyOrigin()    
              .AllowAnyHeader()   
              .AllowAnyMethod());  // Allows any HTTP methods (GET, POST, PUT, DELETE, etc.)
});

// Configuration of JWT (JSON Web Token) Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Sets default authentication scheme
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;    // Sets default challenge scheme for unauthorized requests
})
.AddJwtBearer(options => // Configurating JWT Bearer specific options
{
    options.TokenValidationParameters = new TokenValidationParameters // Defining how the JWT token should be validated
    {
        ValidateIssuer = true,          
        ValidateAudience = true,        
        ValidateLifetime = true,       
        ValidateIssuerSigningKey = true,

        ValidIssuer = "ContactApp",     // Expected issuer of the token (must match token generation)
        ValidAudience = "ContactApp",   // Expected audience of the token (must match token generation)
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration["Jwt:Secret"] ?? "your-secret-key-here-that-is-long-enough-at-least-32-chars" // Key for signing/validating
        ))
    };
});


builder.Services.AddAuthorization();

// Build the application pipeline.
var app = builder.Build();


// Enable Swagger UI in development environment for API documentation
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();     // Serves the Swagger JSON document
    app.UseSwaggerUI();   
}

app.UseHttpsRedirection(); // Redirects HTTP requests to HTTPS

app.UseRouting(); // Adds endpoint routing to the pipeline.

app.UseCors("AllowAllOrigins"); 

app.UseAuthentication(); 
app.UseAuthorization();  

app.UseDefaultFiles(); // Enables serving default file names e.g., index.html, for requests to a directory
app.UseStaticFiles();  // Enables serving static files (CSS, JavaScript, images) from wwwroot

app.MapControllers(); // Maps controller actions to incoming requests

// Database initialization and migrations
// This block ensures the database is created and migrations are applied on application startup.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        dbContext.Database.Migrate(); // Applies any pending migrations to the database
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database."); 
    }
}

app.Run();