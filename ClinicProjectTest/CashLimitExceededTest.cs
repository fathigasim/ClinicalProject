using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectTest
{
    public class CashLimitExceededTest
    {
        [Fact]
        public void CashLimitExceeded_ShouldReturnTrue_WhenCashAndAmountOver1000()
        {
            // Arrange
            var invoice = new Payments
            {
                PaymentMethod = PaymentType.Cash,
                Amount = 1500
            };

            // Act
            var result = invoice.CashLimitExceeded();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CashLimitExceeded_ShouldReturnFalse_WhenCashButAmountUnder1000()
        {
            var invoice = new Payments
            {
                PaymentMethod = PaymentType.Cash,
                Amount = 500
            };

            Assert.False(invoice.CashLimitExceeded());
        }
    }
}
