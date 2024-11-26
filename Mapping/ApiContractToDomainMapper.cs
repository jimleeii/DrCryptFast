using DrCryptFast.Contracts.Requests;
using DrCryptFast.Domain;
using DrCryptFast.Domain.Common;

namespace DrCryptFast.Mapping;

public static class ApiContractToDomainMapper
{
    public static Message ToMessage(this EncryptionRequest request)
    {
        return new Message
        {
            Id = MessageId.From(Guid.NewGuid()),
            Content = Content.From(request.Content),
            EncryptedContent = EncrytedContent.From(request.EncryptedContent)
        };
    }
}