using InventoryApp.Models.Users.User;

namespace InventoryApp.Contracts.Responses
{
    public class GetAdminsResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public UserRole Role { get; set; }
    }
}
