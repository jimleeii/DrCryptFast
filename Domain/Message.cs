using DrCryptFast.Domain.Common;

namespace DrCryptFast.Domain;

public class Message
{
    public MessageId Id { get; set; } = MessageId.From(Guid.NewGuid());

    public Content Content { get; set; } = default!;

    public EncrytedContent EncryptedContent { get; set; } = default!;
}