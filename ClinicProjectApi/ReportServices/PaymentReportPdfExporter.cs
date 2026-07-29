using ClinicProjectApi.GeneratePdf;
using ClinicProjectApplication.Interfaces;
using ClinicProjectApplication.PaymentReports.Dto;
using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApi.ReportServices
{
    public class PaymentReportPdfExporter : IReportExporter<PaymentReportData>
    {
        public string ContentType => "application/pdf";
        public string FileExtension => "pdf";

        public byte[] Export(PaymentReportData data)
        {
            var document = new PaymentReportDocument(data);
            return document.GeneratePdf();
        }
    }
}
