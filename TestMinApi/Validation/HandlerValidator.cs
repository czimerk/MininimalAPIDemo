using FluentValidation;
using Newtonsoft.Json;

namespace TestMinApi.Validation
{
    public class HandlerValidator<T> where T : class
    {
        private readonly IValidator<T> _validator;

        public HandlerValidator(IValidator<T> validator)
        {
            _validator = validator;
        }

        public virtual async Task HandleValidationAsync(T request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync<T>(request);
        }
    }
}
