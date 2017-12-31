using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PBA.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
            : base()
        {
        }

        public ApplicationUser(string userName, string givenName, string familyName, DateTime birthDate)
            : base(userName)
        {
            base.Email = userName;

            GivenName = givenName;
            FamilyName = familyName;
            BirthDate = birthDate;
        }

        [Required]
        [StringLength(50)]
        public string GivenName { get; set; }

        [Required]
        [StringLength(50)]
        public string FamilyName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public string FullName => $"{GivenName} {FamilyName}";
    }
}
