using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Contracts.Parameters.Classroom
{
    public class ClassroomCreateParameter
    {
        [MinLength(2)]
        public string ClassroomName { get; set; }
        [MinLength(2)]
        public string ClassroomDescription { get; set;}
        public string IconUrl { get; set;}
    }
}
