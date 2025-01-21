using dotnetMVC.Data;
using dotnetMVC.Interfaces;
using dotnetMVC.Models;

namespace dotnetMVC.Services
{
    public class CustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _customerRepository.GetAllCustomersAsync();
        }

        public List<Customer> SearchCustomers(string searchString)
        {
            return _customerRepository.SearchCustomers(searchString);
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _customerRepository.GetCustomerByIdAsync(id);
        }

        public async Task AddOrUpdateCustomerAsync(Customer customer)
        {
            // check if creating new or existing customer
            if (customer.Id == 0)
            {
                await _customerRepository.CreateCustomerAsync(customer);
            }
            else
            {
                await _customerRepository.UpdateCustomerAsync(customer);
            }
        }

        public async Task DeleteCustomerAsync(int id)
        {
            await _customerRepository.DeleteCustomerAsync(id);
        }
    }
}
