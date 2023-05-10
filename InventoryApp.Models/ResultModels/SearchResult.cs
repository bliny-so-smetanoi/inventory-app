using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp.Models.ResultModels
{
    
    public class SearchResult : Entity
    {
        public string ClassroomName {get;set;}
        public string Description { get;set;}
        public string IconUrl { get;set;}
        public int NumberOfItems { get;set;}
    }
}
