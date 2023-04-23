using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp.Contracts.Parameters.Item
{
    public class ItemCreateParameter
    {
        public string ItemNumber { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
        public string Condition { get; set; }
        public string Description { get; set; }
        public string ClassroomId { get; set; }
        public string CategoryId { get; set; }
    }
}
