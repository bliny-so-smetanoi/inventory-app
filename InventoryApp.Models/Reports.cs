using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp.Models
{
    public class Reports : Entity
    {
        public string ReportUrl { get; set; }
        [ForeignKey("user_id")]
        public Guid UserId { get; set; }
    }
}
