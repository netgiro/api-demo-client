namespace NetgiroClient.Models
{
    public class CheckoutCallbackModel
    {
        public string TransactionId { get; set; }
        public string ReferenceNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public string Status { get; set; }
        public string AccountNumber { get; set; }
        public string OrderId { get; set; }
        public string ConfirmationCode { get; set; }
        public string Success { get; set; }
        public string PaymentCode { get; set; }
        public string NetgiroSignature { get; set; }
        public string TotalAmount { get; set; }
        public string Signature { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
    }
}
