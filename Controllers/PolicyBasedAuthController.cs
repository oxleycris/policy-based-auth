using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PBA.Models;

namespace PBA.Controllers
{
  [Authorize]
  public class PolicyBasedAuthController : Controller
  {
    private readonly UserManager<ApplicationUser> _userManager;

    public PolicyBasedAuthController(UserManager<ApplicationUser> userManager)
    {
      _userManager = userManager;
    }

    public IActionResult Index()
    {
      return RedirectToAction("IndexAsync");
    }

    public async Task<IActionResult> IndexAsync()
    {

      var user = HttpContext.User;

      var appUser = await _userManager.GetUserAsync(User);

      return View();
    }

    [Authorize(Policy = "IsCris")]
    public IActionResult IsCris()
    {
      return View();
    }

    [Authorize(Policy = "IsCrisEmail")]
    public IActionResult IsCrisEmail()
    {
      return View();
    }

    [Authorize(Policy = "IsOverAge")]
    public IActionResult IsOverAge()
    {
      return View();
    }
  }
}