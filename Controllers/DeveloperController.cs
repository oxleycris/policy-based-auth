using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PBA.Controllers
{
    [Authorize]
    public class DeveloperController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "DevRoleNoClaim")]
        public IActionResult DevRoleNoClaim()
        {
            return View();
        }

        [Authorize(Policy = "DevRoleWithClaim")]
        public IActionResult DevRoleWithClaim()
        {
            return View();
        }
    }
}
