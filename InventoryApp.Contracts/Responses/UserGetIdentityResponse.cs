using InventoryApp.Models;

namespace InventoryApp.Contracts.Responses
{
    public class UserGetIdentityResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public List<Reports> ReportsUrl { get; set; }

    }
}
