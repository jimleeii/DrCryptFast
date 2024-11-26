namespace DrCryptFast.Contracts.Responses;

public class EncryptionResponse
{
    public Guid Id { get; init; }

    public string Content { get; init; } = default!;

    public string EncryptedContent { get; init; } = default!;
}