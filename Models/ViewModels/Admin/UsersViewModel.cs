using System.Collections.Generic;

namespace PBA.Models.ViewModels.Admin
{
    public class UsersViewModel
    {
        public IList<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    }
}
