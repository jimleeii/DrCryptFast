namespace DrCryptFast.Contracts.Requests;

public class EncryptionRequest
{
    public string? Action { get; init; }

    public string Content { get; init; } = default!;

    public string EncryptedContent { get; init; } = default!;
}