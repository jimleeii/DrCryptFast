using FluentValidation;
using FluentValidation.Results;
using ValueOf;

namespace DrCryptFast.Domain.Common;

public class EncrytedContent : ValueOf<string, EncrytedContent>
{
    protected override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Value))
        {
            var message = $"{Value} is not a valid encrytedcontent";
            throw new ValidationException(message, new[]
            {
                new ValidationFailure(nameof(EncrytedContent), message)
            });
        }
    }
}