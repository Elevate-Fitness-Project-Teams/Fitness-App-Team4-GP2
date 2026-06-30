using BuildingBlocks.Shared.Results;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Shared.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Result
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request ,CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .Where(r => r.Errors.Count != 0)
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Count != 0)
            {
                var errors = failures.Select(f => Error.Validation(
                    code: f.PropertyName,
                    description: f.ErrorMessage)).ToList();

                return CreateValidationResult<TResponse>(errors);
            }

            return await next();
        }

      
        private static TResponse CreateValidationResult<TResult>(IEnumerable<Error> errors)
            where TResult : Result
        {
            if (typeof(TResult) == typeof(Result))
            {
                return (TResponse)(object)Result.Fail(errors);
            }

            var resultType = typeof(TResult).GetGenericArguments()[0];
            var failMethod = typeof(Result<>)
                .MakeGenericType(resultType)
                .GetMethod(nameof(Result<int>.Fail), new[] { typeof(IEnumerable<Error>) });

            return (TResponse)failMethod!.Invoke(null, new object[] { errors })!;
        }
    }
}