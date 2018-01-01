using System.Collections.Generic;
using PBA.Models.Admin;

namespace PBA.Models.ViewModels
{
    public class AccessRightsViewModel
    {
        public IList<AccessRightDto> AccessRights { get; set; } = new List<AccessRightDto>();

        public string Name { get; set; }
    }
}
