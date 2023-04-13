using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp.Models.Users.User
{
    public class User : Entity
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        
        public string Password {get;set;}

        [Column(TypeName = "smallint")]
        public UserRole Role { get; set; }
    }
}
