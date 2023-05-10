using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Models.ResultModels
{
    public class StatisticsCategoryPerClassResult 
    {
        [Key]
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
