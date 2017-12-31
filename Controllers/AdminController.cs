using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PBA.Models;
using PBA.Models.Admin;
using PBA.Models.ViewModels.Admin;

namespace PBA.Controllers
{
    [Authorize(Policy = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Users()
        {
            var model = new UsersViewModel { Users = _userManager.Users.ToList() };

            return View(model);
        }

        public IActionResult EditUser(string id)
        {
            // TODO: To come from the database.
            var accessRights = new List<AccessRightDto>
            {
                new AccessRightDto{ Id = 1, Name = "AccessRightA" },
                new AccessRightDto{ Id = 2, Name = "AccessRightB" },
                new AccessRightDto{ Id = 3, Name = "AccessRightC" },
                new AccessRightDto{ Id = 4, Name = "AccessRightD" },
                new AccessRightDto{ Id = 5, Name = "AccessRightE" }
            };

            var model = new EditUserViewModel
            {
                User = _userManager.FindByIdAsync(id).Result,
                AccessRights = accessRights
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult EditUser(EditUserViewModel model)
        {
            var user = _userManager.FindByIdAsync(model.User.Id).Result;
            var currentClaims = _userManager.GetClaimsAsync(model.User).Result;

            foreach (var accessRight in model.AccessRights)
            {
                if (!accessRight.Selected)
                {
                    _userManager.RemoveClaimAsync(user, new Claim(accessRight.Name, "true"));
                }

                if (accessRight.Selected && currentClaims.All(x => x.Type != accessRight.Name))
                {
                    _userManager.AddClaimAsync(user, new Claim(accessRight.Name, "true"));
                }
            }

            return View(model);
        }
    }
}
