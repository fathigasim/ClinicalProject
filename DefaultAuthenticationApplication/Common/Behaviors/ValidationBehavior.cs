
using FluentValidation;
using MediatR;


namespace ClinicProjectApplication.Common.Behaviors
{
    // Application/Common/Behaviors/ValidationBehavior.cs
    public class ValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
            => _validators = validators;

        public async Task<TResponse> Handle(
            TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
        {
            if (!_validators.Any()) return await next();

            var context = new ValidationContext<TRequest>(request);
            var errors = _validators
                .Select(v => v.Validate(context))
                .SelectMany(r => r.Errors)
                .Where(f => f != null).ToList();
                //.GroupBy(f => f.PropertyName)
                //.ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

            if (errors.Any())
                throw new ValidationException(errors);

            return await next();
        }
    }
}
