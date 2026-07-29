namespace ClinicProjectApi.ReportServices
{
    using ClinicProjectApi.GeneratePdf;
    using ClinicProjectApplication.Interfaces;
    using System.Globalization;
    using System.Text;

    public class PaymentReportCsvExporter : IReportExporter<PaymentReportData>
    {
        public string ContentType => "text/csv";
        public string FileExtension => "csv";

        public byte[] Export(PaymentReportData data)
        {
            var sb = new StringBuilder();

            // Header
            sb.AppendLine(string.Join(",",
                "Customer ID", "Invoice No", "Amount", "Paid At", "Payment Method"));

            // Rows
            foreach (var p in data.Payments)
            {
                sb.AppendLine(string.Join(",",
                    p.CustomerId.ToString(CultureInfo.InvariantCulture),
                    EscapeCsvField(p.InvoiceNo),
                    p.Amount.ToString("F2", CultureInfo.InvariantCulture),
                    p.PaidAt.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                    EscapeCsvField(p.PaymentMethod)
                ));
            }

            sb.AppendLine();
            sb.AppendLine($",Total,{data.Total.ToString("F2", CultureInfo.InvariantCulture)},,");

            // UTF-8 BOM — without this, Excel misreads non-ASCII characters (Arabic customer names, invoice notes, etc.) as garbage
            var preamble = Encoding.UTF8.GetPreamble();
            var body = Encoding.UTF8.GetBytes(sb.ToString());
            var result = new byte[preamble.Length + body.Length];
            Buffer.BlockCopy(preamble, 0, result, 0, preamble.Length);
            Buffer.BlockCopy(body, 0, result, preamble.Length, body.Length);
            return result;
        }

        private static string EscapeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field))
                return string.Empty;

            // Must quote if field contains comma, quote, or newline
            bool needsQuoting = field.Contains(',') || field.Contains('"') || field.Contains('\n') || field.Contains('\r');

            if (needsQuoting)
            {
                // Escape embedded quotes by doubling them — this is the actual CSV spec, not optional
                return $"\"{field.Replace("\"", "\"\"")}\"";
            }

            return field;
        }
    }
}
