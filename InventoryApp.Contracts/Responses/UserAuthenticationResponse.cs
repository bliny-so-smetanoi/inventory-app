namespace InventoryApp.Contracts.Responses
{
    public class UserAuthenticationResponse
    {
        public string Token { get; set; }
        public int? Role { get; set; }
    }
}
