using System.Diagnostics;
using dotnetMVC.Interfaces;
using dotnetMVC.Models;
using dotnetMVC.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace dotnetMVC.Controllers
{
    //[Authorize]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CustomerService _customerService;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, CustomerService customerService, IUserService userService)
        {
            _logger = logger;
            _customerService = customerService;
            _userService = userService;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([Bind("Username, Password")] LoginViewModel model)
        {
            // login logic
            if (!ModelState.IsValid)
            {
                return View("Login", model);
            }

            var user = await _userService.AuthenticateAsync(model.Username, model.Password);

            if (user == null)
            {
                ModelState.AddModelError("Password", "Invalid email or password.");
                return View("Login", model);
            }

            var token = _userService.GenerateJwtToken(user);

            // store jwt token in cookie
            Response.Cookies.Append("AuthToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            });

            return View("Index");
        }

        [HttpGet]
        public async Task<IActionResult> CustomerList(string searchString)
        {
            // returns view of all customers / filtered customers
            List<Customer> customers = string.IsNullOrEmpty(searchString)
                ? await _customerService.GetAllCustomersAsync()
                : _customerService.SearchCustomers(searchString);

            return View(customers);
        }

        [HttpGet]
        public async Task<IActionResult> CreateEditCustomer(int? id)
        {
            // create edit view
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
        public async Task<IActionResult> CreateCustomerInfo([Bind("Id, FirstName, LastName, Email")] Customer model)
        {
            // Add or Update new customers
            ModelState.Remove(nameof(model.Id));
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
