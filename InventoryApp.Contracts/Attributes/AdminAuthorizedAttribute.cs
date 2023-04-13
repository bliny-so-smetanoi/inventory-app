using InventoryApp.Models.Users.User;
using Microsoft.AspNetCore.Authorization;

namespace InventoryApp.Contracts.Attributes
{
    public class AdminAuthorizedAttribute : AuthorizeAttribute
    {
        public AdminAuthorizedAttribute(params UserRole[] roles)
        {
            Roles = String.Join(",", roles);
        }
        public AdminAuthorizedAttribute()
        {
            var roles = new UserRole[] { UserRole.Moderator, UserRole.SuperAdmin, UserRole.Admin };
            Roles = String.Join(",", roles);
        }

    }
}
