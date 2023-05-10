using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp.Contracts.Parameters.Item
{
    public class ItemCreateParameter
    {
        [MinLength(2)]
        public string ItemNumber { get; set; }
        [MinLength(2)]
        public string Name { get; set; }
        public string IconUrl { get; set; }
        [MinLength(2)]
        public string Condition { get; set; }
        [MinLength(2)]
        public string Description { get; set; }
        public string ClassroomId { get; set; }
        public string CategoryId { get; set; }
    }
}
