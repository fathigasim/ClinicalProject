namespace ClinicProjectApi.GeneratePdf
{
    public class PaymentReportData
    {
        public DateTime ReportDate { get; set; }
        public List<PaymentReportRow> Payments { get; set; }
        public decimal Total { get; set; }
    }
}
