using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

using Identity.App.Models;
using Microsoft.AspNetCore.Identity;
using static OpenIddict.Abstractions.OpenIddictConstants;
using System.Security.Claims;
using Identity.App.Data;

namespace Identity.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager = null)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            var user = await _userManager.GetUserAsync(User);
            //_userManager.AddClaimAsync(user, new Claim(Permissions.Prefixes.Scope + "sheepla", Permissions.Prefixes.Endpoint + "create"));
            
            var calims = await _userManager.GetClaimsAsync(user);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}