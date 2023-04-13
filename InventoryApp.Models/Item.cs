using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp.Models
{
    public class Item : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        [ForeignKey("classroom_number")]
        public Guid ClassroomNumber { get; set; }

    }
}
