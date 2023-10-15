using FluentResults;
using FluentValidation;
using MediatR;

namespace Application.Validations;

public class ValidationPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse> where TResponse : ResultBase<TResponse>, new()
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPipeline(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators ?? throw new ArgumentNullException(nameof(validators));
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var validationFailures = _validators.Select(validator => validator.Validate(request))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationFailure => validationFailure != null).ToList();

        if (validationFailures.Any())
        {
            var responseType = typeof(TResponse);
            TResponse invalidResponse = new();
            if (responseType.IsGenericType)
            {
                var resultType = responseType.GetGenericArguments()[0];
                var invalidResponseType = typeof(Result<>).MakeGenericType(resultType);

                var nullableInvalidResponse = Activator.CreateInstance(invalidResponseType) as TResponse;

                if (nullableInvalidResponse is not null)
                {
                    invalidResponse = nullableInvalidResponse;
                }
            }

            invalidResponse!.WithErrors(validationFailures.Select(s => s.ErrorMessage));
            return invalidResponse;
        }

        return await next();
    }
}