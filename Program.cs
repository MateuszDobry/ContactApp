using ContactApp.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.Swagger;
// Dodaj dyrektyw� using dla plik�w statycznych
using Microsoft.Extensions.FileProviders; // <-- Nowa linia
using System.IO; // <-- Nowa linia

var builder = WebApplication.CreateBuilder(args);

// Dodaj us�ugi do kontenera DI.

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

// Konfiguracja potoku ��da� HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Przekierowanie HTTP na HTTPS
//app.UseHttpsRedirection();

// U�yj polityki CORS "AllowAllOrigins"
app.UseCors("AllowAllOrigins");

// W��cz routing
app.UseRouting();

// Autoryzacja (na p�niej)
// app.UseAuthentication();
// app.UseAuthorization();

// --- WA�NE: Dodaj te linie do serwowania plik�w statycznych ---
// W��cz serwowanie plik�w statycznych z folderu wwwroot
app.UseDefaultFiles(); // Pozwala na serwowanie index.html jako domy�lnego pliku
app.UseStaticFiles();  // W��cza serwowanie plik�w statycznych

// Je�li chcesz serwowa� pliki z innego miejsca ni� wwwroot, mo�esz u�y�:
// app.UseStaticFiles(new StaticFileOptions
// {
//     FileProvider = new PhysicalFileProvider(
//         Path.Combine(builder.Environment.ContentRootPath, "�cie�ka_do_twojego_frontendu")),
//     RequestPath = "/frontend" // Dost�pne np. pod /frontend/index.html
// });
// ------------------------------------------------------------------

// Mapowanie kontroler�w API
app.MapControllers();

// Inicjalizacja bazy danych i migracje przy starcie aplikacji
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate(); // Automatyczne stosowanie migracji
}

app.Run();