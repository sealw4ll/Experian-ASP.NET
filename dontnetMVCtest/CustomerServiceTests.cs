using Moq;
using dotnetMVC.Models;
using dotnetMVC.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using dotnetMVC.Data;
using dotnetMVC.Interfaces;

namespace dontnetMVCtest
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _mockRepository;
        private readonly CustomerService _customerService;


        public CustomerServiceTests()
        {
            _mockRepository = new Mock<ICustomerRepository>();
            _customerService = new CustomerService(_mockRepository.Object);
        }

        [Fact]
        public async Task AddOrUpdateCustomerAsync_AddsNewCustomerc()
        {
            var newCustomer = new Customer { Id = 0, FirstName = "Tom", LastName = "Holland", Email = "TomHolland@gmail.com" };

            _mockRepository.Setup(repo => repo.CreateCustomerAsync(It.IsAny<Customer>())).Returns(Task.CompletedTask);

            await _customerService.AddOrUpdateCustomerAsync(newCustomer);

            _mockRepository.Verify(repo => repo.CreateCustomerAsync(It.Is<Customer>(c => c.Email == "TomHolland@gmail.com")), Times.Once);
        }

        [Fact]
        public async Task AddOrUpdateCustomerAsync_EditCustomer()
        {
            var newCustomer = new Customer { Id = 0, FirstName = "Lewis", LastName = "Hamilton", Email = "Lewis@gmail.com" };

            _mockRepository.Setup(repo => repo.CreateCustomerAsync(It.IsAny<Customer>())).Returns(Task.CompletedTask);

            await _customerService.AddOrUpdateCustomerAsync(newCustomer);

            _mockRepository.Verify(repo => repo.CreateCustomerAsync(It.Is<Customer>(c => c.Email == "Lewis@gmail.com")), Times.Once);

            newCustomer.Email = "Lewis@Hotmail.com";

            await _customerService.AddOrUpdateCustomerAsync(newCustomer);

            _mockRepository.Verify(repo => repo.CreateCustomerAsync(It.Is<Customer>(c => c.Email == "Lewis@Hotmail.com")), Times.Between(2, 2, Moq.Range.Inclusive));
        }
    }
}