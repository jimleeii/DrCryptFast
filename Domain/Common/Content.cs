using FluentValidation;
using FluentValidation.Results;
using ValueOf;

namespace DrCryptFast.Domain.Common;

public class Content : ValueOf<string, Content>
{
    protected override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Value))
        {
            var message = $"{Value} is not a valid content";
            throw new ValidationException(message, new[]
            {
                new ValidationFailure(nameof(Content), message)
            });
        }
    }
}