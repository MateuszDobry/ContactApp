using ContactApp.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography; 
using System.Text; 

namespace ContactApp.Api.Data
{
   
    // Database context for Entity Framework Core.
    // Manages database interactions for Contact, ContactCategory, ContactSubcategory, and User entities.
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

            // Configure unique email index for contacts to prevent duplicates
            modelBuilder.Entity<Contact>()
                .HasIndex(c => c.Email)
                .IsUnique();

            // Configure unique email index for users for login purposes
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Seed initial data for contact categories
            modelBuilder.Entity<ContactCategory>().HasData(
                new ContactCategory { Id = 1, Nazwa = "S³u¿bowy" }, 
                new ContactCategory { Id = 2, Nazwa = "Prywatny" }, 
                new ContactCategory { Id = 3, Nazwa = "Inny" }      
            );

            // Seed initial data for contact subcategories, linked to 'S³u¿bowy' 
            modelBuilder.Entity<ContactSubcategory>().HasData(
                new ContactSubcategory { Id = 1, Nazwa = "Szef", KategoriaId = 1 },           
                new ContactSubcategory { Id = 2, Nazwa = "Klient", KategoriaId = 1 },         
                new ContactSubcategory { Id = 3, Nazwa = "Wspó³pracownik", KategoriaId = 1 } 
            );

            // A test user with a hashed password
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Imie = "Jan",
                    Email = "jan@example.com",
                    HasloHash = HashPassword("haslo123") // Password "haslo123" is hashed using SHA256
                }
            );

            // A test contacts with a hashed password
     
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
                },
                new Contact
                {
                    Id = 5, 
                    Imie = "Anna",
                    Nazwisko = "Kowalska",
                    Email = "anna.kowalska@example.com",
                    HasloHash = HashPassword("securepass"), 
                    KategoriaId = 1, 
                    PodkategoriaId = 2,
                    Telefon = "444555666",
                    DataUrodzenia = new DateTime(1990, 7, 15) 
                },
                new Contact
                {
                    Id = 6, 
                    Imie = "Piotr",
                    Nazwisko = "Nowak",
                    Email = "piotr.nowak@email.com",
                    HasloHash = HashPassword("P1otrN0w4k"),
                    KategoriaId = 3, 
                    PodkategoriaId = 1, 
                    Telefon = "777888999"
                    // Birth Date can be NULL :)
                },
                new Contact
                {
                    Id = 7, 
                    Imie = "Zofia",
                    Nazwisko = "Wojcik",
                    Email = "zofia.wojcik@mail.pl",
                    HasloHash = HashPassword("Zofia@2024"), 
                    KategoriaId = 2, 
                    Telefon = "123456789",
                    DataUrodzenia = new DateTime(1985, 3, 20)
                }
            );

            // Configure the one-to-many relationship between ContactSubcategory and ContactCategory
            modelBuilder.Entity<ContactSubcategory>()
                .HasOne(sc => sc.Kategoria) // Each subcategory has one category
                .WithMany() // A category can have many subcategories (no explicit navigation property in ContactCategory)
                .HasForeignKey(sc => sc.KategoriaId); // Defines the foreign key
        }

       
        // Helper method to hash a plain-text password using SHA256.
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2")); // Convert byte to hex string
                return builder.ToString();
            }
        }
    }
}