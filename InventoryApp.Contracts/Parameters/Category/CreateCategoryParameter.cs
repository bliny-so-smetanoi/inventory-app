using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Contracts.Parameters.Category
{
    public class CreateCategoryParameter
    {
        [MinLength(2)]
        public string Name { get; set; }
        [MinLength(2)]
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
