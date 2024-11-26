using DrCryptFast.Contracts.Requests;
using FluentValidation;

namespace DrCryptFast.Validation;

public class EncryptionRequestValidator : AbstractValidator<EncryptionRequest>
{
    public EncryptionRequestValidator()
    {
        RuleFor(x => x.Content).NotEmpty();
        RuleFor(x => x.EncryptedContent).NotEmpty();
    }
}