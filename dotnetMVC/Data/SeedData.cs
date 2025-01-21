using System.Security.Cryptography;
using System.Text;
using dotnetMVC.Interfaces;
using dotnetMVC.Models;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;

namespace dotnetMVC.Data;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new CustomerDbContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<CustomerDbContext>>()))
        {
            // seed Customers
            if (context.Customers.Any())
            {
                return;
            }
            context.Customers.AddRange(
                new Customer
                {
                    FirstName = "Max",
                    LastName = "Verstappen",
                    Email = "Max@gmail.com",
                    Id = 1
                },
                new Customer
                {
                    FirstName = "Justin",
                    LastName = "Wong",
                    Email = "JW@gmail.com",
                    Id = 2
                },
                new Customer
                {
                    FirstName = "Flying",
                    LastName = "Tuna",
                    Email = "FlyingTuna@gmail.com",
                    Id = 3
                },
                new Customer
                {
                    FirstName = "Jane",
                    LastName = "Doe",
                    Email = "JaneDoe@gmail.com",
                    Id = 4
                }
            );

            // seed Users
            if (!context.Users.Any())
            {
                var keyBytes = Encoding.UTF8.GetBytes("jdoZApqLuH");
                using var hmac = new HMACSHA256(keyBytes);
                context.Users.AddRange(
                    new User
                    {
                        Username = "user",
                        PasswordHash = Convert.ToBase64String(
                            hmac.ComputeHash(Encoding.UTF8.GetBytes("123"))
                        ),
                        Role = "Admin"
                    }
                );
            }

            context.SaveChanges();
        }
    }
}