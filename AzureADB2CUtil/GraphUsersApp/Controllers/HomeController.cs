using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AADB2C.GraphService;
using Microsoft.AspNetCore.Mvc;
using GraphUsersApp.Models;

namespace GraphUsersApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly B2CGraphClient _graphClient;
        public HomeController(B2CGraphClient graphClient)
        {
            _graphClient = graphClient;
        }
        public async Task<IActionResult> Index()
        {
            var response = await _graphClient.GetAllUsersAsync();

            var accounts = GraphAccounts.Parse(response);
            return View(accounts.value.Select(x => new UserViewModel(x)));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}