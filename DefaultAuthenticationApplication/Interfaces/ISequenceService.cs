using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Interfaces
{
   
    public interface ISequenceService
    {
        Task<string> GenerateOrderNumberAsync();
        Task<string> GenerateInvoiceNumberAsync();
        Task<string> GenerateMedicalNumberAsync();
    }
}
