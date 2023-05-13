namespace InventoryApp.Contracts.Parameters.Item
{
    public class UploadItemsParameter
    {
        public List<IFormFile> files { get; set; }
        public string Owner { get; set; }
    }
}
