using InventoryApp.Models.Users.User;
using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Contracts.Parameters.Admin
{
    public class AdminParameter
    {
        [EmailAddress]
        public string Email { get; set; }
        [MinLength(2)]
        public string FullName { get; set; }
        public UserRole Role { get; set; }
        [MinLength(8)]
        public string Password { get; set; }
    }
}
