using ContactApp.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.Swagger;
// Dodaj dyrektywê using dla plików statycznych
using Microsoft.Extensions.FileProviders; // <-- Nowa linia
using System.IO; // <-- Nowa linia

var builder = WebApplication.CreateBuilder(args);

// Dodaj us³ugi do kontenera DI.

// Konfiguracja bazy danych SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
           .UseLazyLoadingProxies());

// Dodaj kontrolery API
builder.Services.AddControllers();

// Dodaj Swagger/OpenAPI (dla testowania API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Konfiguracja CORS (Cross-Origin Resource Sharing)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                         .AllowAnyHeader()
                         .AllowAnyMethod());
});


var app = builder.Build();

// Konfiguracja potoku ¿¹dañ HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Przekierowanie HTTP na HTTPS
//app.UseHttpsRedirection();

// U¿yj polityki CORS "AllowAllOrigins"
app.UseCors("AllowAllOrigins");

// W³¹cz routing
app.UseRouting();

// Autoryzacja (na póŸniej)
// app.UseAuthentication();
// app.UseAuthorization();

// --- WA¯NE: Dodaj te linie do serwowania plików statycznych ---
// W³¹cz serwowanie plików statycznych z folderu wwwroot
app.UseDefaultFiles(); // Pozwala na serwowanie index.html jako domyœlnego pliku
app.UseStaticFiles();  // W³¹cza serwowanie plików statycznych

// Jeœli chcesz serwowaæ pliki z innego miejsca ni¿ wwwroot, mo¿esz u¿yæ:
// app.UseStaticFiles(new StaticFileOptions
// {
//     FileProvider = new PhysicalFileProvider(
//         Path.Combine(builder.Environment.ContentRootPath, "œcie¿ka_do_twojego_frontendu")),
//     RequestPath = "/frontend" // Dostêpne np. pod /frontend/index.html
// });
// ------------------------------------------------------------------

// Mapowanie kontrolerów API
app.MapControllers();

// Inicjalizacja bazy danych i migracje przy starcie aplikacji
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate(); // Automatyczne stosowanie migracji
}

app.Run();