namespace InventoryApp.Contracts.Dtos
{
    public class EmailDto
    {
        public string FromName { get; set; }
        public string FromAddress { get; set; }
        public string FromPassword { get; set; }
        public string ToName { get; set; }
        public string ToAddress { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }

    }
}
