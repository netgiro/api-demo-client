using System;

namespace NetgiroClient.Models
{
    public class PaymentInfoData
    {
        public string TransactionId { get; set; }

        public int InvoiceNumber { get; set; }

        public string ReferenceNumber { get; set; }

        public int StatusId { get; set; }

        public DateTime Created { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal ShippingAmount { get; set; }

        public decimal HandlingAmount { get; set; }

        public int? AccountNumber { get; set; }

        public int NumberOfInstallments { get; set; }

        public bool? ClientIdentityConfirmed { get; set; }

        public int CartId { get; set; }

        public string LocationId { get; set; }

        public string RegisterId { get; set; }
    }
}