using AutoMapper;
using ClinicProjectApplication.Interfaces;
using ClinicProjectApplication.Payment.Command;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using FluentAssertions;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectTest.PaymentsTests
{
    public class CreatePaymentCommandHandlerTests
    {
        private readonly Mock<IPaymentRepository> _paymentRepoMock = new();
        private readonly Mock<IInvoiceRepository> _invoiceRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IPublisher> _publisherMock = new();

        private readonly CreatePaymentCommandHandler _sut;

        public CreatePaymentCommandHandlerTests()
        {
            _sut = new CreatePaymentCommandHandler(
                _paymentRepoMock.Object,
                _invoiceRepoMock.Object,
                _mapperMock.Object,
                _publisherMock.Object);
        }

        [Fact]
        public async Task Handle_InvoiceNotFound_ReturnsFailure()
        {
            _invoiceRepoMock
                .Setup(r => r.GetInvoiceByInvoiceNumberAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Invoices?)null);

            var command = new CreatePaymentCommand { InvoiceNo = "INV-999" };

            var result = await _sut.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Contain("INV-999");
        }
    }
}
