using System.ComponentModel.DataAnnotations;

namespace PBA.Models
{
    public class AccessRight
    {
        public AccessRight()
        {
            Name = FriendlyName.Replace(" ", string.Empty);
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string FriendlyName { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
