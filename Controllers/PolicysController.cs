using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PBA.Enums;

namespace PBA.Controllers
{
    [Authorize]
    public class PolicysController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = nameof(PolicysEnum.DeveloperRoleNoClaim))]
        public IActionResult DeveloperNoClaimPage()
        {
            return View();
        }

        [Authorize(Policy = nameof(PolicysEnum.DeveloperRoleWithClaim))]
        public IActionResult DeveloperWithClaimPage()
        {
            return View();
        }

        [Authorize(Policy = nameof(PolicysEnum.TesterRoleNoClaim))]
        public IActionResult TesterNoClaimPage()
        {
            return View();
        }

        [Authorize(Policy = nameof(PolicysEnum.TesterRoleWithClaim))]
        public IActionResult TesterWithClaimPage()
        {
            return View();
        }
    }
}
