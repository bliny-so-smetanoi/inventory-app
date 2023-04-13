using InventoryApp.Models.Users.User;

namespace InventoryApp.Contracts.Dtos
{
    public class UserClaimsDto
    {
        public string Email { get; set; }
        public UserRole Role {get;set;}
    }
}
