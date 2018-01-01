using System.ComponentModel.DataAnnotations;

namespace PBA.Enums
{
    public enum AccessRightClaimsEnum
    {
        [Display(Name = "Can Access Developer Site")]
        CanAccessDeveloperSite,

        [Display(Name = "Can Access Tester Site")]
        CanAccessTesterSite,

        [Display(Name = "Can Access Architect Site")]
        CanAccessArchitectSite,

        [Display(Name = "Can Access Manager Site")]
        CanAccessManagerSite
    }
}
