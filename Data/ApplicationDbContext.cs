using ContactApp.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System; // For DateTime in Contact seeding
using System.Security.Cryptography; // Needed for HashPassword if you keep it here
using System.Text; // Needed for HashPassword if you keep it here

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
        public DbSet<User> Users { get; set; }

        // Konfiguracja modelu i seeding danych pocz¹tkowych
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Zapewnienie unikalnoœci adresu email dla Contact
            modelBuilder.Entity<Contact>()
                .HasIndex(c => c.Email)
                .IsUnique();

            // Zapewnienie unikalnoœci adresu email dla User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique(); // It's good practice to ensure unique emails for users too.

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

            // Seeding u¿ytkownika
            // IMPORTANT: For production, NEVER hardcode passwords or hash them directly like this
            // in OnModelCreating. Pre-hash them with a strong algorithm (e.g., BCrypt)
            // and then provide the hash string here.
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Imie = "Jan",
                    Email = "jan@example.com",
                    HasloHash = HashPassword("haslo123") // Using your HashPassword function
                }
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
                    HasloHash = HashPassword("haslo123_contact"), // Hashing for contact too
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
                    HasloHash = HashPassword("haslo456_contact"), // Hashing for contact too
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
                    HasloHash = HashPassword("haslo789_contact"), // Hashing for contact too
                    KategoriaId = 3, // Inny
                    PodkategoriaId = null, // Brak podkategorii ze s³ownika, ale mo¿na by tu wpisaæ dowoln¹
                    Telefon = "555111222",
                    DataUrodzenia = new DateTime(1975, 1, 30)
                }
            );
        }

        // You need to put the HashPassword method OUTSIDE of OnModelCreating,
        // but inside the ApplicationDbContext class.
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }
    }
}