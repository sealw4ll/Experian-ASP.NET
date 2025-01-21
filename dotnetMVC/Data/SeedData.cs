using dotnetMVC.Models;
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
            // Look for any movies.
            if (context.Customers.Any())
            {
                return;   // DB has been seeded
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
            context.SaveChanges();
        }
    }
}