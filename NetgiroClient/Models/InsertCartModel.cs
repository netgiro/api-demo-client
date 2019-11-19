namespace NetgiroClient.Models
{
    public class InsertCartModel
    {
        public int Amount { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public string CustomerId { get; set; }
        public string CallbackUrl { get; set; }
    }
}
