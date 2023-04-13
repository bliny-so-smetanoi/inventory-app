using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Contracts.Parameters
{
    public class UserAuthenticationParameter
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; }   
    }
}
