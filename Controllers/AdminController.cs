using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PBA.Enums;
using PBA.Extensions;
using PBA.Models;
using PBA.Models.Admin;
using PBA.Models.ViewModels;
using PBA.Models.ViewModels.Admin;

namespace PBA.Controllers
{
    [Authorize(Policy = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AdminController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
            var selectedUser = _userManager.FindByIdAsync(id).Result;
            var selectedUserClaims = _userManager.GetClaimsAsync(selectedUser).Result;
            var accessRights = PopulateAccessRights();

            foreach (var accessRight in accessRights)
            {
                if (selectedUserClaims.Any(x => x.Type == accessRight.Name && x.Value == "true"))
                {
                    accessRight.Selected = true;
                }
            }

            var model = new EditUserViewModel
            {
                User = selectedUser,
                AccessRights = accessRights
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = _userManager.FindByIdAsync(model.User.Id).Result;
            var currentClaims = _userManager.GetClaimsAsync(model.User).Result;

            foreach (var accessRight in model.AccessRights)
            {
                if (!accessRight.Selected)
                {
                    await _userManager.RemoveClaimAsync(user, new Claim(accessRight.Name, "true"));
                }

                if (accessRight.Selected && currentClaims.All(x => x.Type != accessRight.Name))
                {
                    await _userManager.AddClaimAsync(user, new Claim(accessRight.Name, "true"));
                }
            }

            // Refresh the claims cookie by refreshing the sign-in process.
            await _signInManager.RefreshSignInAsync(_userManager.FindByNameAsync(User.Identity.Name).Result);

            return View(model);
        }

        private IList<AccessRightDto> PopulateAccessRights()
        {
            return Enum.GetValues(typeof(AccessRightClaimsEnum))
                       .Cast<AccessRightClaimsEnum>()
                       .Select(claim => new AccessRightDto { Name = claim.GetDisplayName() })
                       .ToList();
        }
    }
}
