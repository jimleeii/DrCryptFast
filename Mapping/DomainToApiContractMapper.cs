using DrCryptFast.Contracts.Responses;
using DrCryptFast.Domain;

namespace DrCryptFast.Mapping;

public static class DomainToApiContractMapper
{
    public static EncryptionResponse ToEncryptionResponse(this Message message)
    {
        return new EncryptionResponse
        {
            Id = message.Id.Value,
            Content = message.Content.Value,
            EncryptedContent = message.EncryptedContent.Value
        };
    }
}