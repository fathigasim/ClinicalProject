
using ClinicProjectApplication.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;


namespace ClinicProjectApplication.Common.Behaviors
{
    // Application/Common/Behaviors/TransactionBehavior.cs
    public class TransactionBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<TRequest> _logger;

        private static readonly string RequestName = typeof(TRequest).Name;

        public TransactionBehavior(
            IUnitOfWork uow,
            ILogger<TRequest> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<TResponse> Handle(
       TRequest request,
       RequestHandlerDelegate<TResponse> next,
       CancellationToken cancellationToken)
        {
            if (request is not ITransactionalRequest)
                return await next();

            if (_uow.HasActiveTransaction)
            {
                _logger.LogDebug("Joining existing transaction for {Request}", RequestName);
                return await next();
            }

            _logger.LogDebug("Opening transaction for {Request}", RequestName);

            await _uow.BeginTransactionAsync(cancellationToken);
            try
            {
                var response = await next();
                await _uow.CommitTransactionAsync(cancellationToken);
                _logger.LogDebug("Transaction committed for {Request}", RequestName);
                return response;
            }
            catch (Exception ex)
            {
                await _uow.RollbackTransactionAsync(cancellationToken);
                _logger.LogError(ex, "Transaction rolled back for {Request}", RequestName);
                throw;
            }
        }
    }
}
