namespace NetgiroClient.Models
{
    public class ConfirmCartRequestModel
    {
        /// <summary>
        /// Cart url
        /// </summary>
        public string TransactionId { get; set; }

        /// <summary>
        /// 0 - simulates customer reject payment request, 1- simulates customer confirm payment request
        /// </summary>
        public string Identifier { get; set; }
    }
}
