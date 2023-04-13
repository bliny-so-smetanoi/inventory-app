using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp.Models
{
    public class Image : Entity
    {
        public string Url { get; set; }
        [ForeignKey("entity_id")]
        public Guid EntityId { get; set; }
    }
}
