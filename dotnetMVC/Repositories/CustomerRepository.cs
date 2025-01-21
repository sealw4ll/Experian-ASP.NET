using dotnetMVC.Models;
using dotnetMVC.Interfaces;
using Microsoft.EntityFrameworkCore;
using dotnetMVC.Data;

namespace dotnetMVC.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext _context;

        public CustomerRepository(CustomerDbContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _context.Customers.SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task CreateCustomerAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(c => c.Id == id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }

        public List<Customer> SearchCustomers(string searchString)
        {
            var searchTerms = searchString.ToUpper().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var query = _context.Customers.AsEnumerable();

            var filteredCustomers = query.Where(customer =>
                searchTerms.Any(term =>
                    customer.FirstName!.ToUpper().Contains(term) ||
                    customer.LastName!.ToUpper().Contains(term) ||
                    customer.Email!.ToUpper().Contains(term)
                )
            );

            return filteredCustomers.ToList();
        }
    }
}
