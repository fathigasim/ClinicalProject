using ClinicProjectApi.GeneratePdf;
using ClinicProjectApplication.Interfaces;
using ClosedXML.Excel;

namespace ClinicProjectApi.ReportServices
{
    public class PaymentReportExcelExporter : IReportExporter<PaymentReportData>
    {
        public string ContentType => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public string FileExtension => "xlsx";

        public byte[] Export(PaymentReportData data)
        {
            using var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add("Payments");

            // Header row
            sheet.Cell(1, 1).Value = "Customer ID";
            sheet.Cell(1, 2).Value = "Invoice No";
            sheet.Cell(1, 3).Value = "Amount";
            sheet.Cell(1, 4).Value = "Paid At";
            sheet.Cell(1, 5).Value = "Payment Method";

            var headerRow = sheet.Row(1);
            headerRow.Style.Font.Bold = true;
            headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

            // Data rows
            int row = 2;
            foreach (var p in data.Payments)
            {
                sheet.Cell(row, 1).Value = p.CustomerId;
                sheet.Cell(row, 2).Value = p.InvoiceNo;
                sheet.Cell(row, 3).Value = p.Amount;
                sheet.Cell(row, 3).Style.NumberFormat.Format = "#,##0.00";
                sheet.Cell(row, 4).Value = p.PaidAt;
                sheet.Cell(row, 4).Style.DateFormat.Format = "yyyy-mm-dd hh:mm";
                sheet.Cell(row, 5).Value = p.PaymentMethod;
                row++;
            }

            // Total row
            sheet.Cell(row, 2).Value = "Total";
            sheet.Cell(row, 2).Style.Font.Bold = true;
            sheet.Cell(row, 3).Value = data.Total;
            sheet.Cell(row, 3).Style.Font.Bold = true;
            sheet.Cell(row, 3).Style.NumberFormat.Format = "#,##0.00";

            sheet.Columns().AdjustToContents();
            sheet.SheetView.FreezeRows(1); // headers stay visible on scroll

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
