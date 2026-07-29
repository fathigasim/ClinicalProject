using QuestPDF.Infrastructure;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
namespace ClinicProjectApi.GeneratePdf
{
    public class PaymentReportDocument :IDocument
    {
        private readonly PaymentReportData _data;

        public PaymentReportDocument(PaymentReportData data) => _data = data;

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(10);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Column(col =>
                {
                    col.Item().Text("Payment Report").FontSize(16).Bold();
                    col.Item().Text($"Date: {_data.ReportDate:yyyy-MM-dd}").FontSize(10);
                });

                page.Content().PaddingVertical(15).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2); // Customer
                        columns.RelativeColumn(2); // Invoice
                        columns.RelativeColumn();  // Amount
                        columns.RelativeColumn();  // Method
                        columns.RelativeColumn();  // Date
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text("Customer").Bold();
                        header.Cell().Text("Invoice").Bold();
                        header.Cell().Text("Amount").Bold();
                        header.Cell().Text("Method").Bold();
                        header.Cell().Text("Date").Bold();
                        header.Cell().ColumnSpan(4).PaddingBottom(5).BorderBottom(1);
                    });

                    foreach (var row in _data.Payments)
                    {
                        table.Cell().Text(row.CustomerId);
                        table.Cell().Text(row.InvoiceNo);
                        table.Cell().Text(row.Amount.ToString("C"));
                        table.Cell().Text(row.PaymentMethod);
                        table.Cell().Text(row.PaidAt.ToString("yyyy-MM-dd"));
                    }
                });

                page.Footer().AlignRight().Text(x =>
                {
                    x.Span("Total: ").Bold();
                    x.Span(_data.Total.ToString("C"));
                });
            });
        }
    }
}
