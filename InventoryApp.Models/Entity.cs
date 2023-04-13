using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp.Models
{
    public class Entity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Column(TypeName = "timestamp with time zone")]
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
    }
}
