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

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactCategory> ContactCategories { get; set; }
        public DbSet<ContactSubcategory> ContactSubcategories { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unikalnoœæ emaila dla kontaktów
            modelBuilder.Entity<Contact>()
                .HasIndex(c => c.Email)
                .IsUnique();

            // Unikalnoœæ emaila dla u¿ytkowników
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Seed kategorii
            modelBuilder.Entity<ContactCategory>().HasData(
                new ContactCategory { Id = 1, Nazwa = "S³u¿bowy" },
                new ContactCategory { Id = 2, Nazwa = "Prywatny" },
                new ContactCategory { Id = 3, Nazwa = "Inny" }
            );

            // Seed podkategorii
            modelBuilder.Entity<ContactSubcategory>().HasData(
                new ContactSubcategory { Id = 1, Nazwa = "Szef", KategoriaId = 1 },
                new ContactSubcategory { Id = 2, Nazwa = "Klient", KategoriaId = 1 },
                new ContactSubcategory { Id = 3, Nazwa = "Wspó³pracownik", KategoriaId = 1 }
            );

            // Seed testowego u¿ytkownika
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Imie = "Jan",
                    Email = "jan@example.com",
                    HasloHash = HashPassword("haslo123")
                }
            );

            // Seed testowych kontaktów
            modelBuilder.Entity<Contact>().HasData(
                new Contact
                {
                    Id = 4,
                    Imie = "Test",
                    Nazwisko = "Testowski",
                    Email = "test@test.pl",
                    HasloHash = HashPassword("test123"),
                    KategoriaId = 2,
                    Telefon = "111222333"
                }
            );

            modelBuilder.Entity<ContactSubcategory>()
            .HasOne(sc => sc.Kategoria)
            .WithMany()
            .HasForeignKey(sc => sc.KategoriaId);
        }

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