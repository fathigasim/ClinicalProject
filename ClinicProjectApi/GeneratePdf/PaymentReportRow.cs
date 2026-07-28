namespace ClinicProjectApi.GeneratePdf
{
    public class PaymentReportRow
    {
        public string? CustomerId { get; set; }
        public string InvoiceNo { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaidAt { get; set; }
        public string PaymentMethod { get; set; }
    }
}
