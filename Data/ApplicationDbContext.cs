using ContactApp.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ContactApp.Api.Data
{
    // Kontekst bazy danych dla Entity Framework Core
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Zbiory danych (tabele) w bazie danych
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactCategory> ContactCategories { get; set; }
        public DbSet<ContactSubcategory> ContactSubcategories { get; set; }

        // Konfiguracja modelu i seeding danych pocz¹tkowych
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Zapewnienie unikalnoœci adresu email
            modelBuilder.Entity<Contact>()
                .HasIndex(c => c.Email)
                .IsUnique();

            // Seeding danych dla kategorii kontaktów
            modelBuilder.Entity<ContactCategory>().HasData(
                new ContactCategory { Id = 1, Nazwa = "S³u¿bowy" },
                new ContactCategory { Id = 2, Nazwa = "Prywatny" },
                new ContactCategory { Id = 3, Nazwa = "Inny" }
            );

            // Seeding danych dla podkategorii (przyk³adowe dla "S³u¿bowy")
            modelBuilder.Entity<ContactSubcategory>().HasData(
                new ContactSubcategory { Id = 1, Nazwa = "Szef" },
                new ContactSubcategory { Id = 2, Nazwa = "Klient" },
                new ContactSubcategory { Id = 3, Nazwa = "Wspó³pracownik" },
                new ContactSubcategory { Id = 4, Nazwa = "Dostawca" }
            );

            // Przyk³adowe dane kontaktowe (dla testów)
            // Pamiêtaj, ¿e has³a powinny byæ hashowane w prawdziwej aplikacji!
            // Tutaj u¿ywamy prostego tekstu tylko do szybkiego startu.
            // W rzeczywistoœci u¿y³byœ BCrypt.Net-Next do hashowania.
            modelBuilder.Entity<Contact>().HasData(
                new Contact
                {
                    Id = 1,
                    Imie = "Jan",
                    Nazwisko = "Kowalski",
                    Email = "jan.kowalski@example.com",
                    HasloHash = "haslo123", // Pamiêtaj: to powinno byæ zahashowane!
                    KategoriaId = 1, // S³u¿bowy
                    PodkategoriaId = 2, // Klient
                    Telefon = "123456789",
                    DataUrodzenia = new DateTime(1980, 5, 15)
                },
                new Contact
                {
                    Id = 2,
                    Imie = "Anna",
                    Nazwisko = "Nowak",
                    Email = "anna.nowak@example.com",
                    HasloHash = "haslo456", // Pamiêtaj: to powinno byæ zahashowane!
                    KategoriaId = 2, // Prywatny
                    PodkategoriaId = null, // Brak podkategorii dla prywatnego
                    Telefon = "987654321",
                    DataUrodzenia = new DateTime(1992, 11, 22)
                },
                new Contact
                {
                    Id = 3,
                    Imie = "Piotr",
                    Nazwisko = "Zieliñski",
                    Email = "piotr.zielinski@example.com",
                    HasloHash = "haslo789", // Pamiêtaj: to powinno byæ zahashowane!
                    KategoriaId = 3, // Inny
                    PodkategoriaId = null, // Brak podkategorii ze s³ownika, ale mo¿na by tu wpisaæ dowoln¹
                    Telefon = "555111222",
                    DataUrodzenia = new DateTime(1975, 1, 30)
                }
            );
        }
    }
}