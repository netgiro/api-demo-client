namespace NetgiroClient.Models
{
    public class PaymentResponse
    {
        public bool PaymentSuccessful { get; set; }

        public PaymentInfoData PaymentInfo { get; set; }

        public string Signature { get; set; }

        public bool IsValidSignature { get; set; }

        public bool Success { get; set; }

        public string Message { get; set; }

        public int ResultCode { get; set; }
    }
}
