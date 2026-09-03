using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Enums;
using ClinicProjectDomain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectTest.Invoice
{
    public class InvoiceTest
    {
        [Theory]
        [InlineData(InvoiceStatus.Paid)]
      
        public void InvoiceMarkAsPaid_ShouldThrowInvalidOperationException_WhenMarkedAsPaid(InvoiceStatus newStatus)
        {
            //arrange
            var invoice = Invoices.Create("INV-001", Guid.NewGuid(), 150.00m);
           
                invoice.UpdateStatus(newStatus);

                //act & assert
                Assert.Throws<InvalidOperationException>(() => invoice.MarkAsPaid());

            
       
        }

        [Theory]
    
        [InlineData(InvoiceStatus.Cancelled)]
        public void InvoiceMarkedAsCancelled_ShouldThrowInvalidOperationException_WhenStatusUpdatedToCancelled(InvoiceStatus newStatus)
        {
            //arrange
            var invoice = Invoices.Create("INV-001", Guid.NewGuid(), 150.00m);

            invoice.UpdateStatus(newStatus);

            //act & assert
            Assert.Throws<InvalidOperationException>(() => invoice.MarkAsCancelled());



        }
    }
}
