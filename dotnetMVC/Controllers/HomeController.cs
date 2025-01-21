using System.Diagnostics;
using dotnetMVC.Models;
using dotnetMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace dotnetMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CustomerService _customerService;

        public HomeController(ILogger<HomeController> logger, CustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CustomerList(string searchString)
        {
            List<Customer> customers = string.IsNullOrEmpty(searchString)
                ? await _customerService.GetAllCustomersAsync()
                : _customerService.SearchCustomers(searchString);

            return View(customers);
        }

        [HttpGet]
        public async Task<IActionResult> CreateEditCustomer(int? id)
        {
            Customer? customerInDb = null;
            if (id != null)
            {
                customerInDb = await _customerService.GetCustomerByIdAsync(id.Value);
            }
            return View(customerInDb);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCustomerInfo(int id)
        {
            await _customerService.DeleteCustomerAsync(id);
            return RedirectToAction("CustomerList");
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerInfo([Bind("FirstName, LastName, Email")] Customer model)
        {
            if (ModelState.IsValid)
            { 
                await _customerService.AddOrUpdateCustomerAsync(model);
                return RedirectToAction("CustomerList");
            }

            return View("CreateEditCustomer", model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
