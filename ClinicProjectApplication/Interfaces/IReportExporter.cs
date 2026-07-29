using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Interfaces
{
    public interface IReportExporter<in TReportData>
    {
        string ContentType { get; }
        string FileExtension { get; }
        byte[] Export(TReportData data);
    }
}
