using System.Collections.Generic;
using PBA.Models.Admin;

namespace PBA.Models.ViewModels.Admin
{
    public class EditUserViewModel
    {
        public ApplicationUser User { get; set; }

        public IList<AccessRightDto> AccessRights { get; set; } = new List<AccessRightDto>();
    }
}
